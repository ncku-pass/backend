﻿using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Dto.Messages;
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
    //[Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/experiences")]
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
            var experiencesResponse = await _experienceService.GetExperiencesAsync();
            if (experiencesResponse == null)
            {
                return this.NotFound("查無經歷");
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
            var addTags = await _tagService.TagsExistsAsync(experienceCreateParameter.AddTags);

            if (addTags.Count() > 0)
            {
                string addStr = "";
                addTags.ToList().ForEach(i => addStr += i + ",");
                return this.NotFound($"查無此tags=>\n\tAddTags:{addStr}");
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
            var addTags = await _tagService.TagsExistsAsync(experienceUpdateParameter.AddTags);
            var dropTags = await _tagService.TagsExistsAsync(experienceUpdateParameter.DropTags);
            // TODO:檢查是否已存在關聯
            if (addTags.Count() > 0 || dropTags.Count() > 0)
            {
                string addStr = "";
                string dropStr = "";
                addTags.ToList().ForEach(i => addStr += i + ",");
                dropTags.ToList().ForEach(i => dropStr += i + ",");
                return this.NotFound($"查無此tags=>\n\tAddTags:{addStr}\n\tDropTags:{dropStr}");
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
            var addTags = await _tagService.TagsExistsAsync(experienceUpdateParameter.AddTags ?? new int[] { });
            var dropTags = await _tagService.TagsExistsAsync(experienceUpdateParameter.DropTags ?? new int[] { });
            // TODO:檢查是否已存在關聯
            if (addTags.Count() > 0 || dropTags.Count() > 0)
            {
                string addStr = "";
                string dropStr = "";
                addTags.ToList().ForEach(i => addStr += i + ",");
                dropTags.ToList().ForEach(i => dropStr += i + ",");
                return this.NotFound($"查無此tags=>\n\tAddTags:{addStr}\n\tDropTags:{dropStr}");
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
        [HttpGet("{experienceId}/Tags", Name = "GetTagByExperienceId")]
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