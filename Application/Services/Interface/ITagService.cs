using Application.Dto;
using Application.Dto.Messages;
using Application.Dto.Responses;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface ITagService
    {
        Task<TagResponse> GetTagByIdAsync(int tagId);
        Task<IEnumerable<TagResponse>> GetExperienceTagsAsync(int experienceId);
        Task<bool> TagExistsAsync(int tagId);
        //Task<TagResponse> AddTagAsync(ICollection<TagCreateMessage> tags);
        //Task<bool> DeleteTagAsync(int tagId);

        Tag AddTagAsync(TagCreateMessage tags);

        Task<bool> SaveAsync();
    }
}
