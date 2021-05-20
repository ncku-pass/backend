using Application.Dto.Responses;
using Application.Services.Interface;
using AutoMapper;
using Infrastructure.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Models;
using Application.Dto.Messages;

namespace Application.Services
{
    public class ResumeService : IResumeService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IExperienceService _experienceService;
        private readonly int _userId;

        public ResumeService(
            IHttpContextAccessor httpContextAccessor,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IExperienceService experienceService
            )
        {
            this._httpContextAccessor = httpContextAccessor;
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._experienceService = experienceService;
            this._userId = int.Parse(this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
        }
        /// <summary>
        /// 取得全部履歷
        /// </summary>
        /// <returns></returns>
        public async Task<ICollection<ResumeResponse>> GetResumesAsync()
        {
            var resumeModels = await _unitOfWork.Resume.Where(e => e.UserId == this._userId).ToListAsync();
            var topicModels = await this._unitOfWork.Topic.Where(e => e.UserId == this._userId).ToListAsync();
            var topicIds = topicModels.Select(t => t.Id).ToList();
            var top_ExpModels = await _unitOfWork.Topic_Experience.Where(te => topicIds.Contains(te.TopicId)).ToListAsync();

            var resumeResponses = _mapper.Map<List<ResumeResponse>>(resumeModels);
            var topicResponses = _mapper.Map<List<TopicResponse>>(topicModels);
            var allExpResponses = await this._experienceService.GetExperiencesAsync();

            foreach (var item in topicResponses)
            {
                var expIds = top_ExpModels.Where(te => te.TopicId == item.Id).Select(te => te.ExperienceId).ToList();
                item.Experiences = allExpResponses.Where(e => expIds.Contains(e.Id)).ToList();
            }

            foreach (var resume in resumeResponses)
            {

                resume.Topics = topicResponses.Where(t => t.ResumeId == resume.Id).ToList();
            }

            return resumeResponses;
        }
        /// <summary>
        /// 依Id取得履歷
        /// </summary>
        /// <param name="resumeId"></param>
        /// <returns></returns>
        public async Task<ResumeResponse> GetResumeByIdAsync(int resumeId)
        {
            var resumeModel = await _unitOfWork.Resume.SingleOrDefaultAsync(e => e.UserId == this._userId && e.Id == resumeId);
            var topicModels = await this._unitOfWork.Topic.Where(e => e.ResumeId == resumeId).ToListAsync();
            var topicIds = topicModels.Select(t => t.Id).ToList();
            var top_ExpModels = await _unitOfWork.Topic_Experience.Where(te => topicIds.Contains(te.TopicId)).ToListAsync();

            var resumeResponse = _mapper.Map<ResumeResponse>(resumeModel);
            var topicResponses = _mapper.Map<List<TopicResponse>>(topicModels);
            var allExpResponses = await this._experienceService.GetExperiencesAsync();

            foreach (var topic in topicResponses)
            {
                var expIds = top_ExpModels.Where(te => te.TopicId == topic.Id).Select(te => te.ExperienceId).ToList();
                topic.Experiences = allExpResponses.Where(e => expIds.Contains(e.Id)).ToList();
            }
            resumeResponse.Topics = topicResponses;

            return resumeResponse;
        }
        /// <summary>
        /// 儲存履歷
        /// </summary>
        /// <param name="resumeSaveMessage"></param>
        /// <returns></returns>
        public async Task<ResumeResponse> SaveResumesAsync(ResumeSaveMessage resumeSaveMessage)
        {
            // 建立or更新資料庫的Resume
            var resumeModel = _mapper.Map<Resume>(resumeSaveMessage);
            resumeModel.UserId = this._userId;
            if (resumeModel.Id == 0)//// 新Resume
            {
                this._unitOfWork.Resume.Add(resumeModel);
            }
            else
            {
                this._unitOfWork.Resume.Update(resumeModel);
            }
            //this._unitOfWork.SaveChange();
            await this._unitOfWork.SaveChangeAsync();


            // 建立or更新資料庫的Topic
            var topicMediatorModels = (from topic in resumeSaveMessage.Topics
                                       select new
                                       {
                                           Topic = new Topic { Id = topic.Id, Name = topic.Name, ResumeId = resumeModel.Id, UserId = this._userId },
                                           ExperienceId = topic.ExperienceId
                                       }).ToList();
            foreach (var item in topicMediatorModels)
            {
                if (item.Topic.Id == 0)
                {
                    this._unitOfWork.Topic.Add(item.Topic);
                }
                else
                {
                    this._unitOfWork.Topic.Update(item.Topic);
                }
            }
            //this._unitOfWork.SaveChange();
            await this._unitOfWork.SaveChangeAsync();

            // 建立or刪除資料庫的Topic_Exp關聯
            foreach (var item in topicMediatorModels)
            {
                // 建立待新增/刪除Exp清單
                List<Topic_Experience> currentTop_ExpModels = new List<Topic_Experience> { };
                List<int> currentExpIds = new List<int> { },
                          addTop_ExpIds = new List<int> { },
                          dropTop_ExpIds = new List<int> { }; ;
                if (await this._unitOfWork.Topic_Experience.AnyAsync(te => te.TopicId == item.Topic.Id))
                {
                    currentTop_ExpModels = await this._unitOfWork.Topic_Experience.Where(te => te.TopicId == item.Topic.Id).ToListAsync();
                    currentExpIds = currentTop_ExpModels.Select(te => te.ExperienceId).ToList();
                }
                addTop_ExpIds = item.ExperienceId.Except(currentExpIds).ToList();
                dropTop_ExpIds = currentExpIds.Where(e => !item.ExperienceId.Contains(e)).ToList();

                // 加入新增的Exp關聯
                var addTop_ExpModels = addTop_ExpIds.Select(eId => new Topic_Experience { TopicId = item.Topic.Id, ExperienceId = eId }).ToList();
                this._unitOfWork.Topic_Experience.AddRange(addTop_ExpModels);

                // 刪除被移除的Exp關聯
                var dropTop_ExpModels = currentTop_ExpModels.Where(te => dropTop_ExpIds.Contains(te.ExperienceId)).ToList();
                this._unitOfWork.Topic_Experience.RemoveRange(dropTop_ExpModels);
                await this._unitOfWork.SaveChangeAsync();
            }

            return _ = await GetResumeByIdAsync(resumeModel.Id);
        }
    }
}
