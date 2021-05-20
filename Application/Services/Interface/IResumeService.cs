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
        Task<bool> DeleteResumeAsync(int resumeId);
        Task<bool> DeleteTopicAsync(int resumeId, int topicId);
        Task<bool> ResumeExistsAsync(int resumeId);
        Task<bool> TopicExistsAsync(int resumeId, int topicId);
    }
}