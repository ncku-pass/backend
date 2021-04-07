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
    public class ExperienceService : IExperienceService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;

        public ExperienceService(
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ITagService tagService
            )
        {
            this._httpContextAccessor = httpContextAccessor;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._tagService = tagService;
        }

        /// <summary>
        /// 新增經歷
        /// </summary>
        /// <param name="experienceMessage"></param>
        public async Task<ExperienceResponse> AddExperienceAsync(ExperienceCreateMessage experienceMessage)
        {
            // 新增Exp到資料庫
            var experienceModel = _mapper.Map<Experience>(experienceMessage);
            experienceModel.UserId = int.Parse(this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            this._unitOfWork.Experience.Add(experienceModel);
            await this._unitOfWork.SaveChangeAsync();

            // 新增、刪除Exp_Tag關聯
            ManipulateExp_TagRelation(experienceModel.Id, experienceMessage.AddTags, null);
            await this._unitOfWork.SaveChangeAsync();

            var experienceResponse = _mapper.Map<ExperienceResponse>(experienceModel);
            experienceResponse.Tags = await _tagService.GetExperienceTagsAsync(experienceModel.Id);
            return experienceResponse;
        }

        // TODO:移到Tag_ExpService
        public async void ManipulateExp_TagRelation(int ExperienceId, int[] addTagIds, int[] dropTagIds)
        {
            // 建立待加入的關聯Models，並排除Tag_Exp中已有的Models
            var addTagModels = addTagIds.Select(tid => new Tag_Experience { ExperienceId = ExperienceId, TagId = tid })
                                        .Except(await _unitOfWork.Tag_Experience.Where(n => n.ExperienceId == ExperienceId).ToListAsync())
                                        .ToList();
            _unitOfWork.Tag_Experience.AddRange(addTagModels);

            // 待刪除關聯Model
            if (dropTagIds != null)
            {
                var dropTagModels = await _unitOfWork.Tag_Experience
                                                     .Where(n => n.ExperienceId == ExperienceId && dropTagIds.Contains(n.TagId))
                                                     .ToListAsync();
                _unitOfWork.Tag_Experience.RemoveRange(dropTagModels);
            }
        }

        /// <summary>
        /// 修改經歷
        /// </summary>
        /// <param name="experienceMessage"></param>
        public async Task<ExperienceResponse> UpdateExperienceAsync(ExperienceUpdateMessage experienceUpdateMessage)
        {
            var userId = int.Parse(this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);

            // 取得Exp原檔將Update映射上去
            var experienceModel = await this._unitOfWork.Experience.FirstOrDefaultAsync(n => n.Id == experienceUpdateMessage.Id && n.UserId == userId);
            _mapper.Map(experienceUpdateMessage, experienceModel);

            // 新增、刪除Exp_Tag關聯
            ManipulateExp_TagRelation(experienceModel.Id, experienceUpdateMessage.AddTags, experienceUpdateMessage.DropTags);
            await this._unitOfWork.SaveChangeAsync();

            var experienceResponse = _mapper.Map<ExperienceResponse>(experienceModel);
            experienceResponse.Tags = await _tagService.GetExperienceTagsAsync(experienceModel.Id);
            return experienceResponse;
        }

        /// <summary>
        /// 刪除經歷
        /// </summary>
        /// <param name="experience"></param>
        public async Task<bool> DeleteExperienceAsync(int experienceId)
        {
            // 找出該Exp
            var userId = int.Parse(this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var experienceModel = await this._unitOfWork.Experience.FirstOrDefaultAsync(e => e.Id == experienceId && e.UserId == userId);

            // 移除該Exp全部的Tag關聯
            var experience_TagModels = await this._unitOfWork.Tag_Experience.Where(n => n.ExperienceId == experienceId).ToListAsync();
            this._unitOfWork.Tag_Experience.RemoveRange(experience_TagModels);

            // 移除Exp
            this._unitOfWork.Experience.Remove(experienceModel);
            return await this._unitOfWork.SaveChangeAsync();
        }

        /// <summary>
        /// 依Id查詢經歷是否存在
        /// </summary>
        /// <param name="experienceId"></param>
        /// <returns></returns>
        public async Task<bool> ExperienceExistsAsync(int experienceId)
        {
            var userId = int.Parse(this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return await this._unitOfWork.Experience.AnyAsync(e => e.Id == experienceId && e.UserId == userId);
        }

        /// <summary>
        /// 依Id查詢經歷
        /// </summary>
        /// <param name="experienceId">經歷Id</param>
        /// <returns></returns>
        public async Task<ExperienceResponse> GetExperienceByIdAsync(int experienceId)
        {
            var userId = int.Parse(this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var experienceModel = await this._unitOfWork.Experience.FirstOrDefaultAsync(e => e.Id == experienceId && e.UserId == userId);
            var tagModel = await this._tagService.GetExperienceTagsAsync(experienceId);

            var experienceResponse = _mapper.Map<ExperienceResponse>(experienceModel);
            experienceResponse.Tags = _mapper.Map<ICollection<TagResponse>>(tagModel);

            return experienceResponse;
        }

        /// <summary>
        /// 查詢所有經歷
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ExperienceResponse>> GetExperiencesAsync()
        {
            var userId = int.Parse(this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var experienceModels = await _unitOfWork.Experience.Where(e => e.UserId == userId).ToListAsync();
            var experiencesResponse = _mapper.Map<List<ExperienceResponse>>(experienceModels);
            // TODO:可以將TagSerivce.GetExperienceTagsAsync改成傳入多筆資料
            var tagModel = await (from tag in _unitOfWork.Tag.GetAll()
                                  join _ in _unitOfWork.Tag_Experience.GetAll() on tag.Id equals _.TagId into groupjoin
                                  from combine in groupjoin.DefaultIfEmpty()
                                  select new
                                  {
                                      ExpId = combine.ExperienceId,
                                      Id = tag.Id,
                                      Name = tag.Name
                                  }).OrderBy(t => t.Id).ToListAsync();
            foreach (var exp in experiencesResponse)
            {
                var tagList = tagModel.Where(t => t.ExpId == exp.Id)
                                      .Select(t => new Tag { Id = t.Id, Name = t.Name })
                                      .ToList();
                exp.Tags = _mapper.Map<ICollection<TagResponse>>(tagList);
            }

            return experiencesResponse;
        }
    }
}