using Application.Dto.Messages;
using Application.Dto.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface ITagService
    {
        Task<TagResponse> GetTagByIdAsync(int tagId);

        Task<ICollection<TagResponse>> GetExperienceTagsAsync(int experienceId);

        Task<bool> TagExistsAsync(int tagId);

        Task<ICollection<TagResponse>> GetTagsAsync();

        Task<ICollection<TagResponse>> AddTagAsync(string[] tagNames);

        Task<TagResponse> UpdateTagAsync(TagUpdateMessage updateTagMessage);

        Task<bool> DeleteTagAsync(int tagId);

        Task<ICollection<int>> TagsExistsAsync(int[] tagIds);
    }
}