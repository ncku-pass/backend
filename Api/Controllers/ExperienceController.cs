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
    public class ExperienceController : Controller
    {
        private IExperienceService _experienceService;
        private readonly IMapper _mapper;

        public ExperienceController(
            IExperienceService experienceRepository,
            IMapper mapper
            )
        {
            this._experienceService = experienceRepository;
            this._mapper = mapper;
        }

        [HttpGet(Name = "GetExperienceById")]
        public async Task<IActionResult> GetExperienceById(
            [FromQuery] int experienceId
        )
        {
            var ExperiencesFromService = await _experienceService.GetExperienceAsync(experienceId);
            if (ExperiencesFromService == null)
            {
                return this.NotFound("查無此經歷");
            }
            var ExperienceDto = this._mapper.Map<ExperienceDto>(ExperiencesFromService);
            return this.Ok(ExperienceDto);
        }
    }
}
