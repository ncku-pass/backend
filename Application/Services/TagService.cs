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
        private readonly int _defaultUserId;

        public TagService(
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            this._httpContextAccessor = httpContextAccessor;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._defaultUserId = 1;
            try
            {
                this._userId = int.Parse(this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            catch
            {
                this._userId = 1;
            }
        }

        public async Task<List<TagResponse>> AddTagAsync(string[] tagNames)
        {
            var tagModels = tagNames.Select(tag => new Tag { Id = 0, Name = tag, UserId = this._userId }).ToList();
            this._unitOfWork.Tag.AddRange(tagModels);
            await this._unitOfWork.SaveChangeAsync();
            var tagResponses = this._mapper.Map<List<TagResponse>>(tagModels);
            return tagResponses;
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
            var tagsResponse = await (from tag in _unitOfWork.Tag.Where(t => t.UserId == this._userId || t.UserId == _defaultUserId)
                                      join combine in _unitOfWork.Experience_Tag.Where(t => t.ExperienceId == experienceId)
                                          on tag.Id equals combine.TagId
                                      select new TagResponse()
                                      {
                                          Id = tag.Id,
                                          Name = tag.Name,
                                          IsDefaultTag = (tag.UserId == this._defaultUserId)
                                      }).OrderBy(t => t.Id).ToListAsync();
            return tagsResponse;
        }

        public async Task<TagResponse> GetTagByIdAsync(int tagId)
        {
            var tagModel = await _unitOfWork.Tag.FirstOrDefaultAsync(t => t.Id == tagId && t.UserId == this._userId);
            var tagResponse = _mapper.Map<TagResponse>(tagModel);
            tagResponse.IsDefaultTag = (tagModel.UserId == this._defaultUserId);
            return tagResponse;
        }

        public async Task<ICollection<TagResponse>> GetTagsAsync()
        {
            // 自己的Tag(若使用教發Tag這邊不會篩出)
            var tagModel = await _unitOfWork.Tag.Where(t => t.UserId == this._userId).ToListAsync();
            var tagResponse = _mapper.Map<ICollection<TagResponse>>(tagModel);

            // 使用過且不為自己的的Tag(若有使用教發的Tag則在此篩出)
            // TODO:做法不簡潔
            var expIds = await _unitOfWork.Experience.Where(e => e.UserId == this._userId).Select(e => e.Id).ToListAsync();
            var usedTagIds = await _unitOfWork.Experience_Tag.Where(et => expIds.Contains(et.ExperienceId)).Select(et => et.TagId).Distinct().ToListAsync();
            var defaultTagModel = await _unitOfWork.Tag.Where(t => usedTagIds.Contains(t.Id) && t.UserId != this._userId).ToListAsync();
            var defaultTagResponse = _mapper.Map<ICollection<TagResponse>>(defaultTagModel);
            foreach (var item in defaultTagResponse)
            {
                item.IsDefaultTag = true;
            }

            // 合併回傳
            var result = tagResponse.Union(defaultTagResponse).ToList();
            return result;
        }

        public async Task<bool> TagExistsAsync(int tagId)
        {
            return await _unitOfWork.Tag.AnyAsync(t => t.Id == tagId && (t.UserId == this._userId || t.UserId == _defaultUserId));
        }

        public async Task<TagExistResponse> TagExistAsync(int[] tagIds)
        {
            if (tagIds.Length == 0)
            {
                return new TagExistResponse { Error = false };
            }

            var userTagsList = await _unitOfWork.Tag.Where(t => t.UserId == this._userId || t.UserId == this._defaultUserId).Select(t => t.Id).ToListAsync();
            var notExistTagsIds = tagIds.Except(userTagsList).ToList();

            if (notExistTagsIds.Any())
            {
                return new TagExistResponse
                {
                    Error = true,
                    ErrorMessage = $"查無此Tags=>Ids:{string.Join(", ", notExistTagsIds)}"
                };

            }
            return new TagExistResponse { Error = false };
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
            }).OrderByDescending(t => t.Count).ToList();

            return tagSearchResponse;
        }
    }
}