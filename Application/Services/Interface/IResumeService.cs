using Application.Dto.Messages;
using Application.Dto.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IResumeService
    {
        Task<ICollection<ResumeResponse>> GetResumesAsync();

        Task<ResumeResponse> SaveResumesAsync(ResumeSaveMessage resumeSaveMessage);
    }
}