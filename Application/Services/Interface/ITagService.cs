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

        Task<ICollection<TagResponse>> GetTagsAsync();

        Task<List<TagResponse>> AddTagAsync(string[] tagNames);

        Task<TagResponse> UpdateTagAsync(TagUpdateMessage updateTagMessage);

        Task<bool> DeleteTagAsync(int tagId);

        Task<TagExistResponse> TagExistAsync(int[] tagIds);

        Task<List<TagSearchResponse>> SearchTagAsync(string keyword);
    }
}