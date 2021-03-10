using Application.Services.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections;
using AutoMapper;
using Infrastructure.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Application.Dto;
using Infrastructure.Models;
using Application.Dto.Messages;
using Application.Dto.Responses;

namespace Application.Services
{
    public class ExperienceService : IExperienceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;

        public ExperienceService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ITagService tagService
            )
        {
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
            experienceModel.UserId = 1;
            this._unitOfWork.Experience.Add(experienceModel);
            await this._unitOfWork.SaveChangeAsync();

            // 新增Tags到資料庫
            var tagModels = _mapper.Map<ICollection<Tag>>(experienceMessage.Tags);
            foreach (var tag in tagModels)
            {
                if (tag.Id == 0) // Id為0是EF core預設新增
                {
                    tag.UserId = 1;
                    this._unitOfWork.Tag.Add(tag);
                }
            }
            await this._unitOfWork.SaveChangeAsync();

            // 新增Exp_Tag關聯
            foreach (var tag in tagModels)
            {
                AddTagToExperienceAsync(experienceModel.Id, tag);
            }
            await this._unitOfWork.SaveChangeAsync();

            var experienceResponse = _mapper.Map<ExperienceResponse>(experienceModel);
            experienceResponse.Tags = _mapper.Map<ICollection<TagResponse>>(tagModels);
            return experienceResponse;
        }

        public void AddTagToExperienceAsync(int ExperienceId, Tag tagModels)
        {
            _unitOfWork.Tag_Experience.Add(new Tag_Experience { ExperienceId = ExperienceId, TagId = tagModels.Id });
        }

        public async Task DropTagFromExperienceAsync(int ExperienceId, Tag tagModels)
        {
            var experience_TagModel = await _unitOfWork.Tag_Experience
                                                .FirstOrDefaultAsync(n => n.ExperienceId == ExperienceId && n.TagId == tagModels.Id);
            _unitOfWork.Tag_Experience.Remove(experience_TagModel);
        }

        /// <summary>
        /// 刪除經歷
        /// </summary>
        /// <param name="experience"></param>
        public async Task<bool> DeleteExperienceAsync(int experienceId)
        {
            // 移除該Exp全部的Tag關聯
            var experience_TagModels = await this._unitOfWork.Tag_Experience.Where(n => n.ExperienceId == experienceId).ToListAsync();
            foreach (var exp_tag in experience_TagModels)
            {
                _unitOfWork.Tag_Experience.Remove(exp_tag);
            }

            // 移除Exp
            var experienceModel = await this._unitOfWork.Experience.FirstOrDefaultAsync(e => e.Id == experienceId);
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
            return await this._unitOfWork.Experience.AnyAsync(e => e.Id == experienceId);
        }

        /// <summary>
        /// 依Id查詢經歷
        /// </summary>
        /// <param name="experienceId">經歷Id</param>
        /// <returns></returns>
        public async Task<ExperienceResponse> GetExperienceByIdAsync(int experienceId)
        {
            var experienceModel = await this._unitOfWork.Experience.FirstOrDefaultAsync(n => n.Id == experienceId);
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
            var experienceModels = await _unitOfWork.Experience.ToListAsync();
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
                                  }).ToListAsync();
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
