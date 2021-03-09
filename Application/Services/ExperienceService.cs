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

namespace Application.Services
{
    public class ExperienceService : IExperienceService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExperienceService(
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// 新增經歷
        /// </summary>
        /// <param name="experienceMessage"></param>
        public async Task<ExperienceResponse> AddExperienceAsync(ExperienceCreateMessage experienceMessage)
        {
            var experienceModel = _mapper.Map<Experience>(experienceMessage);
            experienceModel.UserId = 1;
            await this._unitOfWork.Experience.AddAsync(experienceModel);
            return _mapper.Map<ExperienceResponse>(experienceModel);
        }

        /// <summary>
        /// 刪除經歷
        /// </summary>
        /// <param name="experience"></param>
        public async Task<bool> DeleteExperienceAsync(int experienceId)
        {
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
            var experienceModel = await _unitOfWork.Experience.FirstOrDefaultAsync(n => n.Id == experienceId);

            var tagModel = await (from tag in _unitOfWork.Tag.GetAll()
                                  join combine in _unitOfWork.Tag_Experience.Where(t => t.ExperienceId == experienceModel.Id)
                                      on tag.Id equals combine.TagId
                                  select new TagResponse()
                                  {
                                      Id = tag.Id,
                                      Name = tag.Name
                                  }).ToListAsync();

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
            var experienceModel = await _unitOfWork.Experience.ToListAsync();
            var experienceDto = _mapper.Map<List<ExperienceResponse>>(experienceModel);
            var tagModel = await (from tag in _unitOfWork.Tag.GetAll()
                                  join _ in _unitOfWork.Tag_Experience.GetAll() on tag.Id equals _.TagId into groupjoin
                                  from combine in groupjoin.DefaultIfEmpty()
                                  select new
                                  {
                                      ExpId = combine.ExperienceId,
                                      Id = tag.Id,
                                      Name = tag.Name
                                  }).ToListAsync();
            foreach (var exp in experienceDto)
            {
                var tagList = tagModel.Where(t => t.ExpId == exp.Id)
                                      .Select(t => new Tag { Id = t.Id, Name = t.Name })
                                      .ToList();
                exp.Tags = _mapper.Map<ICollection<TagResponse>>(tagList);
            }

            return experienceDto;
        }
    }
}
