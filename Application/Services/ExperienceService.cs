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

            // 新增、刪除Exp_Tag關聯
            ManipulateExp_TagRelation(experienceModel.Id, experienceMessage.AddTags, experienceMessage.DropTags);
            await this._unitOfWork.SaveChangeAsync();

            var experienceResponse = _mapper.Map<ExperienceResponse>(experienceModel);
            experienceResponse.Tags = await _tagService.GetExperienceTagsAsync(experienceModel.Id);
            return experienceResponse;
        }

        public async void ManipulateExp_TagRelation(int ExperienceId, int[] addTagIds, int[] dropTagIds)
        {
            foreach (var id in addTagIds)
            {
                _unitOfWork.Tag_Experience.Add(new Tag_Experience { ExperienceId = ExperienceId, TagId = id });
            }
            foreach (var id in dropTagIds)
            {
                var tagModel = await _unitOfWork.Tag_Experience
                                                .Where(n => n.ExperienceId == ExperienceId && dropTagIds.Contains(n.TagId))
                                                .ToListAsync();
                _unitOfWork.Tag_Experience.RemoveRange(tagModel);
            }
        }

        /// <summary>
        /// 修改經歷
        /// </summary>
        /// <param name="experienceMessage"></param>
        //public async Task<ExperienceResponse> UpdateExperienceAsync(ExperienceUpdateMessage experienceUpdateMessage)
        //{
        //    // 取得Exp原檔將Update Patech映射上去
        //    var experienceModel = await this._unitOfWork.Experience.FirstOrDefaultAsync(n => n.Id == experienceUpdateMessage.Id);
        //    _mapper.Map(experienceUpdateMessage, experienceModel);

        //    // Tag分類   
        //    var tagCreateModels = new List<Tag>();      // 全新Tags新增、加關聯
        //    var tagDropRelateModels = new List<Tag>();  // 現有Tags移除關聯
        //    var tagAddRelateModels = new List<Tag>();   // 現有Tags加關聯

        //    foreach (var tag in experienceUpdateMessage.Tags)
        //    {
        //        // Id為0是EF core預設新增
        //        if (tag.Id == 0)
        //        {
        //            tagCreateModels.Add(_mapper.Map<Tag>(tag));
        //        }
        //        else if (tag.IsDrop)
        //        {
        //            tagDropRelateModels.Add(_mapper.Map<Tag>(tag));
        //        }
        //        else
        //        {
        //            tagAddRelateModels.Add(_mapper.Map<Tag>(tag));
        //        }
        //    }

        //    // 全新Tags新增到資料庫
        //    await this._unitOfWork.SaveChangeAsync();

        //    // 新增Exp_Tag關聯
        //    foreach (var tag in tagModels)
        //    {
        //        AddTagToExperience(experienceModel.Id, tag);
        //    }
        //    await this._unitOfWork.SaveChangeAsync();

        //    var experienceResponse = _mapper.Map<ExperienceResponse>(experienceModel);
        //    experienceResponse.Tags = _mapper.Map<ICollection<TagResponse>>(tagModels);
        //    return experienceResponse;
        //}

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