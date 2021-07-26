using Application.Dto.Messages;
using Application.Dto.Responses;
using Application.Services.Interface;
using AutoMapper;
using Infrastructure.Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TagService : ITagService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly int _userId;

        public TagService(
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            this._httpContextAccessor = httpContextAccessor;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._userId = int.Parse(this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }

        public async Task<ICollection<TagResponse>> AddTagAsync(string[] tagNames)
        {
            var tagModels = tagNames.Select(tag => new Tag { Id = 0, Name = tag, UserId = this._userId }).ToList();
            this._unitOfWork.Tag.AddRange(tagModels);
            await this._unitOfWork.SaveChangeAsync();
            var tagResponse = this._mapper.Map<ICollection<TagResponse>>(tagModels);
            return tagResponse;
        }

        public async Task<bool> DeleteTagAsync(int tagId)
        {
            //找出該Tag
            var tagModel = await _unitOfWork.Tag.FirstOrDefaultAsync(t => t.Id == tagId && t.UserId == this._userId);

            // 移除該Exp全部的Tag關聯
            var tag_ExperienceModels = await this._unitOfWork.Experience_Tag.Where(n => n.TagId == tagId).ToListAsync();
            this._unitOfWork.Experience_Tag.RemoveRange(tag_ExperienceModels);

            // 移除Tag
            this._unitOfWork.Tag.Remove(tagModel);
            return await this._unitOfWork.SaveChangeAsync();
        }

        public async Task<ICollection<TagResponse>> GetExperienceTagsAsync(int experienceId)
        {
            var tagsResponse = await (from tag in _unitOfWork.Tag.Where(t => t.UserId == this._userId)
                                      join combine in _unitOfWork.Experience_Tag.Where(t => t.ExperienceId == experienceId)
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
            var tagModel = await _unitOfWork.Tag.FirstOrDefaultAsync(t => t.Id == tagId && t.UserId == this._userId);
            var tagResponse = _mapper.Map<TagResponse>(tagModel);
            return tagResponse;
        }

        public async Task<ICollection<TagResponse>> GetTagsAsync()
        {
            var tagModel = await _unitOfWork.Tag.Where(t => t.UserId == this._userId).ToListAsync();
            var tagResponse = _mapper.Map<ICollection<TagResponse>>(tagModel);
            return tagResponse;
        }

        public async Task<bool> TagExistsAsync(int tagId)
        {
            return await _unitOfWork.Tag.AnyAsync(t => t.Id == tagId && t.UserId == this._userId);
        }

        public async Task<ICollection<int>> TagsExistsAsync(int[] tagIds)
        {
            if (tagIds.Length <= 0)
            {
                return new List<int> { };
            }
            var userTagsList = await _unitOfWork.Tag.Where(t => t.UserId == this._userId).Select(t => t.Id).ToListAsync();
            var tagNotExist = tagIds.Except(userTagsList).ToList();
            return tagNotExist;
        }

        public async Task<TagResponse> UpdateTagAsync(TagUpdateMessage updateTagMessage)
        {
            var tagModel = await _unitOfWork.Tag.SingleOrDefaultAsync(t => t.Id == updateTagMessage.Id && t.UserId == this._userId);
            this._mapper.Map(updateTagMessage, tagModel);
            await this._unitOfWork.SaveChangeAsync();
            var tagResponse = _mapper.Map<TagResponse>(tagModel);
            return tagResponse;
        }

        public async Task<List<TagSearchResponse>> SearchTagAsync(string keyword)
        {
            // 撈出含有關鍵字的Tag
            var tagModels = await _unitOfWork.Tag.Where(t => t.Name.Contains(keyword)).ToListAsync();

            // 整理格式成TagSearchResponse
            var tagSearchResponse = new List<TagSearchResponse>();
            foreach (var tag in tagModels)
            {
                tagSearchResponse.Add(new TagSearchResponse
                {
                    Id = tag.UserId == this._userId ? tag.Id : 0, // 是自己的Tag保留Id，否則設為0
                    Name = tag.Name,
                    Count = await this._unitOfWork.Experience_Tag.CountAsync(et => et.TagId == tag.Id)
                });
            }

            // GroupBy TagName並加總Id和count
            tagSearchResponse = tagSearchResponse.GroupBy(x => x.Name, (name, tag) => new TagSearchResponse
            {
                Id = tag.Sum(t => t.Id), // 加總Id，若不是自己的則加總為0
                Name = name,
                Count = tag.Sum(t => t.Count)
            }).OrderBy(t => t.Count).ToList();

            return tagSearchResponse;
        }
    }
}