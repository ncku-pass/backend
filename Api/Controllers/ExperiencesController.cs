using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Dto.Messages;
using Application.Dto.Responses;
using Application.Services.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly IImageService _imageService;

        public ExperiencesController(
            IExperienceService experienceRepository,
            IDefaultDataService defaultDataService,
            IMapper mapper,
            ITagService tagService,
            IImageService imageService
            )
        {
            this._experienceService = experienceRepository;
            this._defaultDataService = defaultDataService;
            this._mapper = mapper;
            this._tagService = tagService;
            this._imageService = imageService;
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
            var tagExistResponse = await _tagService.TagExistAsync(experienceCreateParameter.Tags);
            if (tagExistResponse.Error)
            {
                return this.NotFound($"{tagExistResponse.ErrorMessage}");
            }

            // TODO:統一try catch處裡contoller
            try
            {
                await this._imageService.ImgExistAsync(experienceCreateParameter.Images);
            }
            catch (ArgumentException ex)
            {
                return this.NotFound(ex.Message);
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
            var tagExistResponse = await _tagService.TagExistAsync(experienceUpdateParameter.Tags);
            if (tagExistResponse.Error)
            {
                return this.NotFound($"{tagExistResponse.ErrorMessage}");
            }

            // TODO:統一try catch處裡contoller
            try
            {
                await this._imageService.ImgExistAsync(experienceUpdateParameter.Images);
            }
            catch (ArgumentException ex)
            {
                return this.NotFound(ex.Message);
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
            var tagExistResponse = await _tagService.TagExistAsync(experienceUpdateParameter.Tags);
            if (tagExistResponse.Error)
            {
                return this.NotFound($"{tagExistResponse.ErrorMessage}");
            }

            // TODO:統一try catch處裡contoller
            try
            {
                await this._imageService.ImgExistAsync(experienceUpdateParameter.Images);
            }
            catch (ArgumentException ex)
            {
                return this.NotFound(ex.Message);
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

        /// <summary>
        /// 刪除多筆經歷
        /// </summary>
        /// <param name="deleteParameter"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteExperience([FromBody] ExperienceDeleteParameter deleteParameter)
        {
            // 確認Exp存在且屬於該User
            var notExistExpIds = await _experienceService.ExperiencesExistsAsync(deleteParameter.Ids);
            if (notExistExpIds.Any())
            {
                return this.NotFound("查無此經歷=>Id:" + notExistExpIds);
            }

            // 逐筆刪除
            foreach (var expId in deleteParameter.Ids)
            {
                await _experienceService.DeleteExperienceAsync(expId);
            }
            return this.NoContent();
        }

        /// get api/experiences/1
        /// <summary>
        /// 根據經歷Id取得tag
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

        /// PUT /api/experiences/tags
        /// <summary>
        /// 批次編輯Exp Tag關聯
        /// </summary>
        /// <param name="relateTagsParameter"></param>
        /// <returns></returns>
        [HttpPut("tags")]
        public async Task<IActionResult> EditExperienceRelateTags([FromBody] ExperienceRelateTagsParameter[] relateTagsParameter)
        {
            // 確認Exp存在且屬於該User
            var notExistExpIds = await _experienceService.ExperiencesExistsAsync(relateTagsParameter.Select(p => p.ExperienceId).ToArray());
            if (notExistExpIds.Any())
            {
                return this.NotFound("查無此經歷=>Id:" + notExistExpIds);
            }

            // 更新Exp Tag關聯
            foreach (var item in relateTagsParameter)
            {
                await this._experienceService.ManipulateExp_TagRelation(item.ExperienceId, item.TagIds);
            }

            // 整理回傳
            var expResponse = await _experienceService.GetExperiencesAsync();
            var relateTagsViewModels = new List<ExperienceRelateTagsViewModel>();
            foreach (var item in relateTagsParameter)
            {
                relateTagsViewModels.Add(new ExperienceRelateTagsViewModel
                {
                    ExperienceId = item.ExperienceId,
                    TagIds = expResponse.Single(e => e.Id == item.ExperienceId).Tags.Select(t => t.Id).ToArray()
                });
            }

            return this.Ok(relateTagsViewModels);
        }
    }
}