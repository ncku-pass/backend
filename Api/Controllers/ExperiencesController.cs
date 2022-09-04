using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Dto.Messages;
using Application.Dto.Responses;
using Application.Services.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/experiences")]
    public class ExperiencesController : Controller
    {
        private readonly IExperienceService _experienceService;
        private readonly IDefaultDataService _defaultDataService;
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;

        public ExperiencesController(
            IExperienceService experienceRepository,
            IDefaultDataService defaultDataService,
            IMapper mapper,
            ITagService tagService
            )
        {
            this._experienceService = experienceRepository;
            this._defaultDataService = defaultDataService;
            this._mapper = mapper;
            this._tagService = tagService;
        }

        /// get api/experiences
        /// <summary>
        /// 取得所有經歷
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetExperiences()
        {
            var experiencesResponse = await _experienceService.GetExperiencesAsync();
            if (!experiencesResponse.Any())
            {
                // 若使用者沒有經歷，則新增範例經歷
                await this._defaultDataService.CreateGuideExampleAsync();
                experiencesResponse = await _experienceService.GetExperiencesAsync();
            }
            var experienceViewModel = this._mapper.Map<List<ExperienceViewModel>>(experiencesResponse);
            var experienceClassifiedViewModel = this._mapper.Map<ExperienceClassifiedViewModel>(experienceViewModel);
            return this.Ok(experienceClassifiedViewModel);
        }

        /// get api/experiences/1
        /// <summary>
        /// 根據Id取得經歷
        /// </summary>
        /// <param name="experienceId">經歷Id</param>
        /// <returns></returns>
        [HttpGet("{experienceId}", Name = "GetExperienceById")]
        public async Task<IActionResult> GetExperienceById([FromRoute] int experienceId)
        {
            if (!await _experienceService.ExperienceExistsAsync(experienceId))
            {
                return this.NotFound("查無此經歷=>Id:" + experienceId);
            }
            var experienceResponse = await _experienceService.GetExperienceByIdAsync(experienceId);
            var experienceViewModel = this._mapper.Map<ExperienceViewModel>(experienceResponse);
            return this.Ok(experienceViewModel);
        }

        /// <summary>
        /// 新增經歷
        /// </summary>
        /// <param name="experienceCreateParameter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateExperience([FromBody] ExperienceCreateParameter experienceCreateParameter)
        {
            var addTags = await _tagService.TagsExistsAsync(experienceCreateParameter.Tags);

            if (addTags.Count() > 0)
            {
                string addStr = "";
                addTags.ToList().ForEach(i => addStr += i + ",");
                return this.NotFound($"查無此tags=>\n\tAddTags:{addStr}");
            }

            var experienceMessage = _mapper.Map<ExperienceCreateMessage>(experienceCreateParameter);
            var experienceResponse = await _experienceService.AddExperienceAsync(experienceMessage);
            var experienceViewModel = _mapper.Map<ExperienceViewModel>(experienceResponse);
            return this.CreatedAtRoute(
                "GetExperienceById",
                new { experienceId = experienceViewModel.Id },
                experienceViewModel
            );
        }

        /// <summary>
        /// 匯入經歷資料
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [HttpPost("import")]
        public async Task<IActionResult> ImportExperience([FromBody] List<ExperienceImportParameter> parameter)
        {
            var experienceMessage = _mapper.Map<List<ExperienceCreateMessage>>(parameter);
            var experieceResponses = new List<ExperienceResponse>();
            foreach (var item in experienceMessage)
            {
                experieceResponses.Add(await _experienceService.AddExperienceAsync(item));
            }
            var experienceViewModels = _mapper.Map<List<ExperienceViewModel>>(experieceResponses);
            return this.Ok(experienceViewModels);
        }

        /// <summary>
        /// 完整更新經歷
        /// </summary>
        /// <param name="experienceId"></param>
        /// <param name="experienceUpdateParameter"></param>
        /// <returns></returns>
        [HttpPut("{experienceId}")]
        public async Task<IActionResult> UpdateExperience(
            [FromRoute] int experienceId,
            [FromBody] ExperienceUpdateParameter experienceUpdateParameter
        )
        {
            if (!await _experienceService.ExperienceExistsAsync(experienceId))
            {
                return this.NotFound("查無此經歷=>Id:" + experienceId);
            }
            // 檢查Tag是否皆存在
            var notExistTags = await _tagService.TagsExistsAsync(experienceUpdateParameter.Tags);
            if (notExistTags.Count() > 0)
            {
                string notExistTagsStr = "";
                notExistTags.ToList().ForEach(i => notExistTagsStr += i + ",");
                return this.NotFound($"查無此tags=>\n\tTags:{notExistTagsStr}");
            }

            // 更新此Exp
            var experienceUpdateMessage = _mapper.Map<ExperienceUpdateMessage>(experienceUpdateParameter);
            experienceUpdateMessage.Id = experienceId;
            var experienceResponse = await _experienceService.UpdateExperienceAsync(experienceUpdateMessage);

            var experienceViewModel = _mapper.Map<ExperienceViewModel>(experienceResponse);
            return this.CreatedAtRoute(
                "GetExperienceById",
                new { experienceId = experienceViewModel.Id },
                experienceViewModel
            );
        }

        /// <summary>
        /// 部分更新經歷
        /// </summary>
        /// <param name="experienceId"></param>
        /// <param name="patchDocument"></param>
        /// <returns></returns>
        [HttpPatch("{experienceId}")]
        public async Task<IActionResult> PartiallyExperience(
            [FromRoute] int experienceId,
            [FromBody] JsonPatchDocument<ExperienceUpdateParameter> patchDocument
        )
        {
            if (!await _experienceService.ExperienceExistsAsync(experienceId))
            {
                return this.NotFound("查無此經歷=>Id:" + experienceId);
            }

            // 搜出指定待更新Exp
            var experienceToPatch = await _experienceService.GetExperienceByIdAsync(experienceId);
            var experienceUpdateParameter = _mapper.Map<ExperienceUpdateParameter>(experienceToPatch);
            patchDocument.ApplyTo(experienceUpdateParameter, ModelState);

            // 檢查Tag是否皆存在
            var notExistTags = await _tagService.TagsExistsAsync(experienceUpdateParameter.Tags);
            if (notExistTags.Count() > 0)
            {
                string notExistTagsStr = "";
                notExistTags.ToList().ForEach(i => notExistTagsStr += i + ",");
                return this.NotFound($"查無此tags=>\n\tTags:{notExistTagsStr}");
            }

            // 更新此Exp
            var experienceUpdateMessage = _mapper.Map<ExperienceUpdateMessage>(experienceUpdateParameter);
            experienceUpdateMessage.Id = experienceId;
            var experienceResponse = await _experienceService.UpdateExperienceAsync(experienceUpdateMessage);

            var experienceViewModel = _mapper.Map<ExperienceViewModel>(experienceResponse);
            return this.CreatedAtRoute(
                "GetExperienceById",
                new { experienceId = experienceViewModel.Id },
                experienceViewModel
            );
        }

        /// <summary>
        /// 刪除經歷
        /// </summary>
        /// <param name="experienceId"></param>
        /// <returns></returns>
        [HttpDelete("{experienceId}")]
        public async Task<IActionResult> DeleteExperience([FromRoute] int experienceId)
        {
            if (!await _experienceService.ExperienceExistsAsync(experienceId))
            {
                return this.NotFound("查無此經歷=>Id:" + experienceId);
            }
            await _experienceService.DeleteExperienceAsync(experienceId);
            return this.NoContent();
        }

        /// get api/experiences/1
        /// <summary>
        /// 根據經歷Id取得經歷
        /// </summary>
        /// <param name="experienceId">經歷Id</param>
        /// <returns></returns>
        [HttpGet("{experienceId}/tags", Name = "GetTagByExperienceId")]
        public async Task<IActionResult> GetTagByExperienceId([FromRoute] int experienceId)
        {
            if (!await _experienceService.ExperienceExistsAsync(experienceId))
            {
                return this.NotFound("查無此經驗=>Id:" + experienceId);
            }
            var tagResponse = await _tagService.GetExperienceTagsAsync(experienceId);
            var tagViewModels = _mapper.Map<ICollection<TagViewModel>>(tagResponse);
            return this.Ok(tagViewModels);
        }
    }
}