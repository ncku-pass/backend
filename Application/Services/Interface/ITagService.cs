using Application.Dto.Messages;
using Application.Dto.Responses;
using Infrastructure.Models;
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


        Task<ICollection<Tag>> AddTagAsync(string[] tagNames);

        Task<bool> DeleteTagAsync(int tagId);

        Task<ICollection<int>> TagsExistsAsync(int[] tagIds);

    }
}