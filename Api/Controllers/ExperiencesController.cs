using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Dto;
using Application.Dto.Messages;
using Application.Services.Interface;
using AutoMapper;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using System;
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

        public ExperiencesController(
            IExperienceService experienceRepository,
            IMapper mapper
            )
        {
            this._experienceService = experienceRepository;
            this._mapper = mapper;
        }

        /// get api/experiences
        /// <summary>
        /// 取得所有經歷
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetExperiences()
        {
            var ExperiencesFromService = await _experienceService.GetExperiencesAsync();
            if (ExperiencesFromService == null)
            {
                return this.NotFound("查無經歷");
            }
            var ExperienceDto = this._mapper.Map<List<ExperienceResponse>>(ExperiencesFromService);
            return this.Ok(ExperienceDto);
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
            var ExperienceResponse = await _experienceService.GetExperienceByIdAsync(experienceId);
            if (ExperienceResponse == null)
            {
                return this.NotFound("查無此經歷=>Id:" + experienceId);
            }
            var ExperienceViewModel = this._mapper.Map<ExperienceViewModel>(ExperienceResponse);
            return this.Ok(ExperienceViewModel);
        }

        //Task<bool> ExperienceExistsAsync(int experienceId);
        //void AddExperience(ExperienceDtoCreate experience);
        //void DeleteExperienceAsync(int experienceId);

        [HttpPost]
        public async Task<IActionResult> CreateExperience([FromBody] ExperienceCreateParameter experienceCreateParameter)
        {
            var experienceModel = _mapper.Map<ExperienceCreateMessage>(experienceCreateParameter);
            var experienceResponse = await _experienceService.AddExperienceAsync(experienceModel);
            var experienceViewModel = _mapper.Map<ExperienceViewModel>(experienceResponse);
            return this.CreatedAtRoute(
                "GetExperienceById",
                new { experienceId = experienceViewModel.Id },
                experienceViewModel
            );
        }
    }
}
