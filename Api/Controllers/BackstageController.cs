using Api.RequestModel.Parameters;
using Application.Dto.Messages;
using Application.Services.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Authorize(Roles = "Dashboard")]
    [Route("api/backstage")]
    [ApiController]
    public class BackstageController : ControllerBase
    {
        private readonly IBackstageService _backstageService;
        private readonly IMapper _mapper;

        public BackstageController(
            IBackstageService backstageService,
            IMapper mapper
            )
        {
            this._backstageService = backstageService;
            this._mapper = mapper;
        }

        [HttpPost("analyze-categories")]
        public async Task<IActionResult> CategoriesAnalyze([FromBody] BackstageCategoriesAnalyzeParameter parameter)
        {
            var message = this._mapper.Map<BackstageCategoriesAnalyzeMessage>(parameter);
            var response = await this._backstageService.CategoriesAnalyze(message);

            return this.Ok(response);
        }

        [HttpPost("analyze-abilities")]
        public async Task<IActionResult> AbilityAnalyze([FromBody] BackstageCategoriesAnalyzeParameter parameter)
        {
            var message = this._mapper.Map<BackstageCategoriesAnalyzeMessage>(parameter);
            var response = await this._backstageService.AbilityAnalyze(message);

            return this.Ok(response);
        }
    }
}