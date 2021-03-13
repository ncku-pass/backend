using Application.Dto.Messages;
using Application.Dto.Responses;
using Application.Services.Interface;
using AutoMapper;
using Infrastructure.Infrastructure;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TagService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<ICollection<TagResponse>> AddTagAsync(string[] tagNames)
        {
            var tagModels = tagNames.Select(tag => new Tag { Id = 0, Name = tag, UserId = 1 }).ToList();
            this._unitOfWork.Tag.AddRange(tagModels);
            await this._unitOfWork.SaveChangeAsync();
            var tagResponse = this._mapper.Map<ICollection<TagResponse>>(tagModels);
            return tagResponse;
        }

        public async Task<bool> DeleteTagAsync(int tagId)
        {
            // 移除該Exp全部的Tag關聯
            var tag_ExperienceModels = await this._unitOfWork.Tag_Experience.Where(n => n.TagId == tagId).ToListAsync();
            this._unitOfWork.Tag_Experience.RemoveRange(tag_ExperienceModels);

            // 移除Tag
            var tagModel = await _unitOfWork.Tag.FirstOrDefaultAsync(t => t.Id == tagId);
            this._unitOfWork.Tag.Remove(tagModel);
            return await this._unitOfWork.SaveChangeAsync();
        }

        // TODO:獨立出Tag_Experience Service
        public async Task<ICollection<TagResponse>> GetExperienceTagsAsync(int experienceId)
        {
            var tagsResponse = await (from tag in _unitOfWork.Tag.GetAll()
                                      join combine in _unitOfWork.Tag_Experience.Where(t => t.ExperienceId == experienceId)
                                          on tag.Id equals combine.TagId
                                      select new TagResponse()
                                      {
                                          Id = tag.Id,
                                          Name = tag.Name
                                      }).OrderBy(t => t.Id).ToListAsync();
            return tagsResponse;
        }

        public async Task<TagResponse> GetTagByIdAsync(int tagId)
        {
            var tagModel = await _unitOfWork.Tag.FirstOrDefaultAsync(t => t.Id == tagId);
            var tagResponse = _mapper.Map<TagResponse>(tagModel);
            return tagResponse;
        }

        public async Task<ICollection<TagResponse>> GetTagsAsync()
        {
            var tagModel = await _unitOfWork.Tag.GetAll().ToListAsync();
            var tagResponse = _mapper.Map<ICollection<TagResponse>>(tagModel);
            return tagResponse;
        }

        public async Task<bool> TagExistsAsync(int tagId)
        {
            return await _unitOfWork.Tag.AnyAsync(t => t.Id == tagId);
        }

        public async Task<ICollection<int>> TagsExistsAsync(int[] tagIds)
        {
            if (tagIds.Length <= 0)
            {
                return new List<int> { };
            }
            var tagNotExist = tagIds.Except(await _unitOfWork.Tag.GetAll().Select(t => t.Id).ToListAsync()).ToList();
            return tagNotExist;
        }

        public async Task<TagResponse> UpdateTagAsync(TagUpdateMessage updateTagMessage)
        {
            var tagModel = await _unitOfWork.Tag.SingleOrDefaultAsync(t => t.Id == updateTagMessage.Id);
            this._mapper.Map(updateTagMessage, tagModel);
            await this._unitOfWork.SaveChangeAsync();
            var tagResponse = _mapper.Map<TagResponse>(tagModel);
            return tagResponse;
        }
    }
}