using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Dto.Messages;
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
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/portfolios")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IExperienceService _experienceService;
        private readonly IPortfolioService _portfolioService;
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;

        public PortfolioController(
            IExperienceService experienceRepository,
            IPortfolioService portfolioService,
            IMapper mapper,
            ITagService tagService
            )
        {
            this._experienceService = experienceRepository;
            this._portfolioService = portfolioService;
            this._mapper = mapper;
            this._tagService = tagService;
        }

        /// get api/portfolios
        /// <summary>
        /// 取得所有履歷
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetResumes()
        {
            var resumeResponse = await _portfolioService.GetResumesAsync();
            if (resumeResponse == null)
            {
                return this.NotFound("查無履歷");
            }
            var resumeViewModel = this._mapper.Map<List<ResumeViewModel>>(resumeResponse);
            return this.Ok(resumeResponse);
        }

        // post api/portfolios/{id}/save
        /// <summary>
        /// 儲存履歷
        /// </summary>
        /// <param name="resumeId"></param>
        /// <param name="portfolioSaveParameter"></param>
        /// <returns></returns>
        [HttpPost("{resumeId}/Save")]
        public async Task<IActionResult> SaveResumes(
            [FromRoute] int resumeId,
            [FromBody] PortfolioSaveParameter portfolioSaveParameter
            )
        {
            var expNotExist = await this._experienceService.ExperiencesExistsAsync(PickExpInTopic(portfolioSaveParameter));

            if (expNotExist.Count() > 0)
            {
                string expStr = "";
                expNotExist.ToList().ForEach(i => expStr += i + ",");
                return this.NotFound($"查無此exps=>{expStr}");
            }

            var portfolioSaveMessage = _mapper.Map<PortfolioSaveMessage>(portfolioSaveParameter);
            portfolioSaveMessage.Id = resumeId;
            var portfolioResponse = await this._portfolioService.SaveResumesAsync(portfolioSaveMessage);

            return this.Ok(portfolioResponse);
        }

        int[] PickExpInTopic(PortfolioSaveParameter portfolioSaveParameter)
        {
            List<int> expIds = new List<int>();
            foreach (var topic in portfolioSaveParameter.Topics)
            {
                expIds.AddRange(topic.ExperienceId);
            }
            return expIds.Distinct().ToArray();
        }
    }
}
