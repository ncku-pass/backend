﻿using Application.Dto.Messages;
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
        private readonly int _userId;

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
            this._userId = int.Parse(this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
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
            await ManipulateExp_TagRelation(experienceModel.Id, experienceMessage.Tags);

            var experienceResponse = _mapper.Map<ExperienceResponse>(experienceModel);
            experienceResponse.Tags = await _tagService.GetExperienceTagsAsync(experienceModel.Id);
            return experienceResponse;
        }

        // TODO:移到Tag_ExpService
        public async Task ManipulateExp_TagRelation(int experienceId, int[] tagIds)
        {
            // 建立待加入的關聯Models，並排除Tag_Exp中已有的Models
            var currentTagIds = await _unitOfWork.Experience_Tag.Where(n => n.ExperienceId == experienceId).ToListAsync();
            var addTagModels = tagIds.Except(currentTagIds.Select(t => t.TagId))
                                     .Select(tid => new Experience_Tag { ExperienceId = experienceId, TagId = tid })
                                     .ToList();
            var dropTagModels = currentTagIds.Where(t => !tagIds.Contains(t.TagId))
                                             .ToList();

            if (addTagModels.Count() != 0 || dropTagModels.Count() != 0)
            {
                _unitOfWork.Experience_Tag.AddRange(addTagModels);
                _unitOfWork.Experience_Tag.RemoveRange((IEnumerable<Experience_Tag>)dropTagModels);
                await this._unitOfWork.SaveChangeAsync();
            }
        }

        /// <summary>
        /// 修改經歷
        /// </summary>
        /// <param name="experienceMessage"></param>
        public async Task<ExperienceResponse> UpdateExperienceAsync(ExperienceUpdateMessage experienceUpdateMessage)
        {
            // 取得Exp原檔將Update映射上去
            var experienceModel = await this._unitOfWork.Experience.FirstOrDefaultAsync(n => n.Id == experienceUpdateMessage.Id && n.UserId == this._userId);
            _mapper.Map(experienceUpdateMessage, experienceModel);
            await this._unitOfWork.SaveChangeAsync();

            // 新增、刪除Exp_Tag關聯
            await ManipulateExp_TagRelation(experienceModel.Id, experienceUpdateMessage.Tags);

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
            var experienceModel = await this._unitOfWork.Experience.FirstOrDefaultAsync(e => e.Id == experienceId && e.UserId == this._userId);

            // 移除該Exp全部的Tag關聯
            var experience_TagModels = await this._unitOfWork.Experience_Tag.Where(n => n.ExperienceId == experienceId).ToListAsync();
            this._unitOfWork.Experience_Tag.RemoveRange((IEnumerable<Experience_Tag>)experience_TagModels);

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
            return await this._unitOfWork.Experience.AnyAsync(e => e.Id == experienceId && e.UserId == this._userId);
        }

        /// <summary>
        /// 依Id List查詢經歷是否存在
        /// </summary>
        /// <param name="expIds"></param>
        /// <returns></returns>
        public async Task<ICollection<int>> ExperiencesExistsAsync(int[] expIds)
        {
            if (expIds.Length <= 0)
            {
                return new List<int> { };
            }
            var userExpsList = await _unitOfWork.Experience.Where(t => t.UserId == this._userId).Select(t => t.Id).ToListAsync();
            var expNotExist = expIds.Except(userExpsList).ToList();
            return expNotExist;
        }

        /// <summary>
        /// 依Id查詢經歷
        /// </summary>
        /// <param name="experienceId">經歷Id</param>
        /// <returns></returns>
        public async Task<ExperienceResponse> GetExperienceByIdAsync(int experienceId)
        {
            var experienceModel = await this._unitOfWork.Experience.FirstOrDefaultAsync(e => e.Id == experienceId && e.UserId == this._userId);
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
            var experienceModels = await _unitOfWork.Experience.Where(e => e.UserId == this._userId).ToListAsync();
            var experiencesResponse = _mapper.Map<List<ExperienceResponse>>(experienceModels);
            // TODO:可以將TagSerivce.GetExperienceTagsAsync改成傳入多筆資料
            var tagModel = await (from tag in _unitOfWork.Tag.GetAll()
                                  join _ in _unitOfWork.Experience_Tag.GetAll() on tag.Id equals _.TagId into groupjoin
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