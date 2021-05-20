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
            return this.Ok(resumeResponse);
        }

        // post api/resumes/{id}/save
        /// <summary>
        /// 儲存履歷
        /// </summary>
        /// <param name="resumeId"></param>
        /// <param name="resumeSaveParameter"></param>
        /// <returns></returns>
        [HttpPost("{resumeId}/Save")]
        public async Task<IActionResult> SaveResumes(
            [FromRoute] int resumeId,
            [FromBody] ResumeSaveParameter resumeSaveParameter
            )
        {
            if (!await _resumeService.ResumeExistsAsync(resumeId))
            {
                return this.NotFound("查無此=>resumeId:" + resumeId);
            }

            var expNotExist = await this._experienceService.ExperiencesExistsAsync(PickExpInTopic(resumeSaveParameter));
            if (expNotExist.Count() > 0)
            {
                string expStr = "";
                expNotExist.ToList().ForEach(i => expStr += i + ",");
                return this.NotFound($"查無此exps=>{expStr}");
            }

            var resumeSaveMessage = _mapper.Map<ResumeSaveMessage>(resumeSaveParameter);
            resumeSaveMessage.Id = resumeId;
            var resueResponse = await this._resumeService.SaveResumesAsync(resumeSaveMessage);

            return this.Ok(resueResponse);
        }

        private int[] PickExpInTopic(ResumeSaveParameter resumeSaveParameter)
        {
            List<int> expIds = new List<int>();
            foreach (var topic in resumeSaveParameter.Topics)
            {
                expIds.AddRange(topic.ExperienceId);
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

        // post api/resumes/{resumeId}/topics/{topicId}
        /// <summary>
        /// 刪除主題
        /// </summary>
        /// <param name="resumeId"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        [HttpDelete("{resumeId}/topics/{topicId}")]
        public async Task<IActionResult> DeleteTopicById(
            [FromRoute] int resumeId,
            [FromRoute] int topicId
            )
        {
            if (!await _resumeService.ResumeExistsAsync(resumeId))
            {
                return this.NotFound("查無此=>resumeId:" + resumeId);
            }
            if (!await _resumeService.TopicExistsAsync(resumeId, topicId))
            {
                return this.NotFound("查無此=>topicId:" + topicId);
            }

            await this._resumeService.DeleteTopicAsync(resumeId, topicId);

            return this.NoContent();
        }
    }
}