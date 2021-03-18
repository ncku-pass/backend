using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Dto.Messages;
using Application.Dto.Responses;
using Application.Services.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private IExperienceService _experienceService;
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;

        public TagsController(
            IExperienceService experienceRepository,
            IMapper mapper,
            ITagService tagService
            )
        {
            this._experienceService = experienceRepository;
            this._mapper = mapper;
            this._tagService = tagService;
        }

        /// get api/tags
        /// <summary>
        /// 取得所有標籤
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            var tagResponse = await _tagService.GetTagsAsync();
            if (tagResponse == null)
            {
                return this.NotFound("查無標籤");
            }
            var tagViewModel = this._mapper.Map<List<TagViewModel>>(tagResponse);
            return this.Ok(tagViewModel);
        }

        /// get api/experiences/1
        /// <summary>
        /// 根據標籤Id取得經歷
        /// </summary>
        /// <param name="tagId">標籤Id</param>
        /// <returns></returns>
        [HttpGet("{tagId}", Name = "GetTagById")]
        public async Task<IActionResult> GetTagById([FromRoute] int tagId)
        {
            if (!await _tagService.TagExistsAsync(tagId))
            {
                return this.NotFound("查無此標籤=>Id:" + tagId);
            }
            var tagResponse = await _tagService.GetTagByIdAsync(tagId);
            var tagViewModel = _mapper.Map<TagViewModel>(tagResponse);
            return this.Ok(tagViewModel);
        }



        /// <summary>
        /// 新增標籤
        /// </summary>
        /// <param name="tagNames">標籤名稱集合</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateTags([FromBody] string[] tagNames)
        {
            if (tagNames.Length <= 0)
            {
                return this.NotFound("tagNames為空");
            }
            var tagResponse = await _tagService.AddTagAsync(tagNames);
            var tagViewModel = _mapper.Map<ICollection<TagViewModel>>(tagResponse);
            return this.Ok(tagViewModel);
        }

        /// <summary>
        /// 修改標籤
        /// </summary>
        /// <param name="tagId">目標修改Id</param>
        /// <param name="tagName">新標籤名稱</param>
        /// <returns></returns>
        [HttpPut("{tagId}")]
        public async Task<IActionResult> UpdateTags(
            [FromRoute] int tagId,
            [FromBody] string tagName
            )
        {
            if (!await _tagService.TagExistsAsync(tagId))
            {
                return this.NotFound("查無此標籤=>Id:" + tagId);
            }
            var tagResponse = await _tagService.UpdateTagAsync(new TagUpdateMessage { Id = tagId, Name = tagName });
            var tagViewModel = _mapper.Map<TagViewModel>(tagResponse);
            return this.CreatedAtRoute(
                "GetTagById",
                new { tagId = tagViewModel.Id },
                tagViewModel
            );
        }

        /// <summary>
        /// 刪除經歷
        /// </summary>
        /// <param name="tagId">指定刪除標籤Id</param>
        /// <returns></returns>
        [HttpDelete("{tagId}")]
        public async Task<IActionResult> DeleteExperience([FromRoute] int tagId)
        {
            if (!await _tagService.TagExistsAsync(tagId))
            {
                return this.NotFound("查無此標籤=>Id:" + tagId);
            }
            var experienceResponse = await _tagService.DeleteTagAsync(tagId);
            return this.NoContent();
        }
    }
}
