using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Dto.Messages;
using Application.Dto.Responses;
using Application.Services.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class ExperiencesController : Controller
    {
        private IExperienceService _experienceService;
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;

        public ExperiencesController(
            IExperienceService experienceRepository,
            IMapper mapper,
            ITagService tagService
            )
        {
            this._experienceService = experienceRepository;
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
            var ExperiencesResponse = await _experienceService.GetExperiencesAsync();
            if (ExperiencesResponse == null)
            {
                return this.NotFound("查無經歷");
            }
            var ExperienceViewModel = this._mapper.Map<List<ExperienceViewModel>>(ExperiencesResponse);
            return this.Ok(ExperienceViewModel);
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
            var ExperienceResponse = await _experienceService.GetExperienceByIdAsync(experienceId);
            var ExperienceViewModel = this._mapper.Map<ExperienceViewModel>(ExperienceResponse);
            return this.Ok(ExperienceViewModel);
        }

        /// <summary>
        /// 新增經歷
        /// </summary>
        /// <param name="experienceCreateParameter"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateExperience([FromBody] ExperienceCreateParameter experienceCreateParameter)
        {
            var AddTags = await _tagService.TagsExistsAsync(experienceCreateParameter.AddTags);
            var DropTags = await _tagService.TagsExistsAsync(experienceCreateParameter.DropTags);

            if (AddTags.Count() > 0 || DropTags.Count() > 0)
            {
                string addStr = "";
                string dropStr = "";
                AddTags.ToList().ForEach(i => addStr += i + ",");
                DropTags.ToList().ForEach(i => dropStr += i + ",");
                return this.NotFound($"查無此tags=>\n\tAddTags:{addStr}\n\tDropTags:{dropStr}");
            }

            var experienceModel = _mapper.Map<ExperienceCreateMessage>(experienceCreateParameter);
            var experienceResponse = await _experienceService.AddExperienceAsync(experienceModel);
            var experienceViewModel = _mapper.Map<ExperienceViewModel>(experienceResponse);
            return this.CreatedAtRoute(
                "GetExperienceById",
                new { experienceId = experienceViewModel.Id },
                experienceViewModel
            );
        }

        //[HttpPut("{experienceId}")]
        //public async Task<IActionResult> UpdateExperience(
        //    [FromRoute] int experienceId,
        //    [FromBody] ExperienceUpdateParameter experienceUpdateParameter
        //)
        //{
        //    if (!await _experienceService.ExperienceExistsAsync(experienceId))
        //    {
        //        return this.NotFound("查無此經歷=>Id:" + experienceId);
        //    }

        //    var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(touristRouteId);
        //    // 1. 映射Dto
        //    // 2. 更新Dto
        //    // 3. 映射Model
        //    // mapper一步做完
        //    _mapper.Map(touristRouteForUpdateDto, touristRouteFromRepo);

        //    await _touristRouteRepository.SaveAsync();

        //    return this.NoContent();
        //}

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
            var experienceResponse = await _experienceService.DeleteExperienceAsync(experienceId);
            return this.NoContent();
        }
    }
}