using Api.RequestModel.Parameters;
using Application.Dto.Messages;
using Application.Services.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Authorize]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/ncku-protal")]
    [ApiController]
    public class NCKUPortalController : ControllerBase
    {
        public readonly INCKUPortalService _NCKUPortalService;
        private readonly IMapper _mapper;

        public NCKUPortalController(
            INCKUPortalService nCKUPortalService,
            IMapper mapper
            )
        {
            this._NCKUPortalService = nCKUPortalService;
            this._mapper = mapper;
        }

        /// <summary>
        /// 取得在校經歷
        /// </summary>
        /// <param name="type">["all", "activity", "course", "club"]</param>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        [HttpPost("exp-record")]
        public async Task<IActionResult> GetExpRecords(
            [FromQuery] string type,
            [FromBody] NCKUPortalTokenParameter Parameter
            )
        {
            var message = this._mapper.Map<NCKUPortalTokenMessage>(Parameter);
            var response = await this._NCKUPortalService.GetRecord(message, type);
            return this.Ok(response);
        }
    }
}