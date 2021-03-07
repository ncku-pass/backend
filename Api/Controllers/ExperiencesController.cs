using Application.Dtos;
using Application.Services.Interface;
using AutoMapper;
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
            var ExperienceDto = this._mapper.Map<List<ExperienceDto>>(ExperiencesFromService);
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
            var ExperiencesFromService = await _experienceService.GetExperienceByIdAsync(experienceId);
            if (ExperiencesFromService == null)
            {
                return this.NotFound("查無此經歷=>Id:" + experienceId);
            }
            var ExperienceDto = this._mapper.Map<ExperienceDto>(ExperiencesFromService);
            return this.Ok(ExperienceDto);
        }

        //Task<bool> ExperienceExistsAsync(int experienceId);
        //void AddExperience(ExperienceDtoCreate experience);
        //void DeleteExperienceAsync(int experienceId);

        //[HttpPost]
        //public async Task<IActionResult> CreateExperience([FromBody] ExperienceDtoCreate experienceDtoCreate)
        //{
        //    _experienceService.AddExperience(experienceDtoCreate);
        //    var touristRouteReturn = _mapper.Map<ExperienceDto>(experienceDtoCreate);
        //    return this.CreatedAtRoute(
        //        "GetTouristRouteById",
        //        new { touristRouteId = touristRouteReturn.Id },
        //        touristRouteReturn
        //    );

        //}
    }
}
