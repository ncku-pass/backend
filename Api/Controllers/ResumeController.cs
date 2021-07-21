using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Dto.Messages;
using Application.Services.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/resumes")]
    [ApiController]
    public class ResumeController : ControllerBase
    {
        private readonly IExperienceService _experienceService;
        private readonly IResumeService _resumeService;
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;

        public ResumeController(
            IExperienceService experienceRepository,
            IResumeService resumeService,
            IMapper mapper,
            ITagService tagService
            )
        {
            this._experienceService = experienceRepository;
            this._resumeService = resumeService;
            this._mapper = mapper;
            this._tagService = tagService;
        }

        /// get api/resumes
        /// <summary>
        /// 取得所有履歷
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResumes()
        {
            var resumeResponse = await _resumeService.GetResumesAsync();
            if (resumeResponse == null)
            {
                return this.NotFound("查無履歷");
            }
            var resumeViewModel = this._mapper.Map<List<ResumeViewModel>>(resumeResponse);
            return this.Ok(resumeViewModel);
        }

        /// get api/resumes
        /// <summary>
        /// 取得指定Id履歷
        /// </summary>
        /// <returns></returns>
        [HttpGet("{resumeId}", Name = "GetResumeIdById")]
        public async Task<IActionResult> GetResumesById([FromRoute] int resumeId)
        {
            if (!await _resumeService.ResumeExistsAsync(resumeId))
            {
                return this.NotFound("查無此經歷=>Id:" + resumeId);
            }
            var resumeResponse = await _resumeService.GetResumeByIdAsync(resumeId);
            var resumeViewModel = this._mapper.Map<ResumeViewModel>(resumeResponse);
            return this.Ok(resumeViewModel);
        }

        // post api/resumes/{id}/save
        /// <summary>
        /// 儲存履歷
        /// </summary>
        /// <param name="resumeId"></param>
        /// <param name="resumeSaveParameter"></param>
        /// <returns></returns>
        [HttpPost("{resumeId}")]
        public async Task<IActionResult> SaveResumes(
            [FromRoute] int resumeId,
            [FromBody] ResumeSaveParameter resumeSaveParameter
            )
        {
            if (!await _resumeService.ResumeExistsAsync(resumeId) && resumeId != 0)
            {
                return this.NotFound("查無此=>resumeId:" + resumeId);
            }

            // 檢查是否有不存在的Exp
            var expNotExist = await this._experienceService.ExperiencesExistsAsync(PickExpInCard(resumeSaveParameter));
            if (expNotExist.Count() > 0)
            {
                string expStr = "";
                expNotExist.ToList().ForEach(i => expStr += i + ",");
                return this.NotFound($"查無此exps=>{expStr}");
            }

            var resumeSaveMessage = _mapper.Map<ResumeSaveMessage>(resumeSaveParameter);
            resumeSaveMessage.Id = resumeId;
            var resumeResponse = await this._resumeService.SaveResumeAsync(resumeSaveMessage);

            return this.Ok(resumeResponse);
        }

        private int[] PickExpInCard(ResumeSaveParameter resumeSaveParameter)
        {
            List<int> expIds = new List<int>();
            foreach (var card in resumeSaveParameter.Cards)
            {
                var expIdInCard = card.Experiences.Select(e => e.Id).ToList();
                expIds.AddRange(expIdInCard);
            }
            return expIds.Distinct().ToArray();
        }

        // post api/resumes/{resumeId}
        /// <summary>
        /// 刪除履歷
        /// </summary>
        /// <param name="resumeId"></param>
        /// <returns></returns>
        [HttpDelete("{resumeId}")]
        public async Task<IActionResult> DeleteResumeById(
            [FromRoute] int resumeId
            )
        {
            if (!await _resumeService.ResumeExistsAsync(resumeId))
            {
                return this.NotFound("查無此=>resumeId:" + resumeId);
            }

            await this._resumeService.DeleteResumeAsync(resumeId);
            return this.NoContent();
        }

        //// post api/resumes/{resumeId}/topics/{topicId}
        ///// <summary>
        ///// 刪除主題
        ///// </summary>
        ///// <param name="resumeId"></param>
        ///// <param name="topicId"></param>
        ///// <returns></returns>
        //[HttpDelete("{resumeId}/topics/{topicId}")]
        //public async Task<IActionResult> DeleteTopicById(
        //    [FromRoute] int resumeId,
        //    [FromRoute] int topicId
        //    )
        //{
        //    if (!await _resumeService.ResumeExistsAsync(resumeId))
        //    {
        //        return this.NotFound("查無此=>resumeId:" + resumeId);
        //    }
        //    if (!await _resumeService.TopicExistsAsync(resumeId, topicId))
        //    {
        //        return this.NotFound("查無此=>topicId:" + topicId);
        //    }

        //    await this._resumeService.DeleteTopicAsync(resumeId, topicId);

        //    return this.NoContent();
        //}
    }
}