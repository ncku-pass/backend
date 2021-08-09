using Application.Dto.Messages;
using Application.Dto.Responses;
using Application.Services.Interface;
using Infrastructure.Infrastructure;
using Infrastructure.Service.Interface;
using JWT.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NCKUPortalService : INCKUPortalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INCKUPortalAPI _NCKUPortalAPI;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NCKUPortalService(
            IUnitOfWork unitOfWork,
            INCKUPortalAPI NCKUPortalAPI,
            IHttpContextAccessor httpContextAccessor
            )
        {
            this._unitOfWork = unitOfWork;
            this._NCKUPortalAPI = NCKUPortalAPI;
            this._httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 驗證計中Token
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<NCKUPortalTokenVerifyResponse> TokenVerify(NCKUPortalTokenMessage message)
        {
            var response = await this._NCKUPortalAPI.GetExpRecordAsync(message.Key, message.KeyVal, "all");
            if (response == "{\"msg\":\"key and keyval no paired!!\"}")
            {
                return new NCKUPortalTokenVerifyResponse() { Succeeded = false, ErrorMessage = response };
            }

            var payload = JwtBuilder.Create()
                                    .Decode<IDictionary<string, object>>(message.KeyVal);
            var verifyResponse = new NCKUPortalTokenVerifyResponse()
            {
                Succeeded = true,
                StudentId = payload["commonname"].ToString().ToLower(),
                Email = (payload["commonname"].ToString() + "@gs.ncku.edu.tw").ToLower(),
                Name = payload["DisplayName"].ToString(),
                Major = payload["studentdeptNo"].ToString(),
                EnrollmentYear = int.Parse("1" + payload["commonname"].ToString().Substring(3, 2)) // 從學號取入學年，未處裡民國100年前
            };

            return verifyResponse;
        }

        /// <summary>
        /// 驗證使用者
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<bool> UserVerify(NCKUPortalTokenMessage message)
        {
            var userId = int.Parse(this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // 驗證計中Token是否與屬於當前系統使用者，避免冒用
            var outerStudentId = JwtBuilder.Create()
                                           .Decode<IDictionary<string, object>>(message.KeyVal)["commonname"]
                                           .ToString()
                                           .ToLower();
            var innerStudentId = (await this._unitOfWork.User.FirstOrDefaultAsync(u => u.Id == userId)).StudentId;
            return outerStudentId == innerStudentId;
        }

        /// <summary>
        /// 取得在校活動紀錄
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type">["all", "activity", "course", "club"]</param>
        /// <returns></returns>
        public async Task<string> GetRecord(NCKUPortalTokenMessage message, string type)
        {
            if (!await this.UserVerify(message))
            {
                return "StudentId not paired!";
            }

            var response = await this._NCKUPortalAPI.GetExpRecordAsync(message.Key, message.KeyVal, type);
            return response;
        }
    }
}