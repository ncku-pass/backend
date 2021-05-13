using Api.RequestModel.ViewModels;
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
    [Route("api/[controller]")]
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

        /// get api/Portfolios
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
            // TODO:此非ViewModel待轉換
            //var resumeViewModel = this._mapper.Map<List<ResumeViewModel>>(resumeResponse);
            return this.Ok(resumeResponse);
        }
    }
}
