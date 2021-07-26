using Application.Dto.Messages;
using Application.Dto.Responses;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface INCKUPortalService
    {
        Task<NCKUPortalTokenVerifyResponse> TokenVerify(NCKUPortalTokenMessage message);

        Task<bool> UserVerify(NCKUPortalTokenMessage message);

        Task<string> GetRecord(NCKUPortalTokenMessage message, string type);
    }
}