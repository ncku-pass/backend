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

namespace Application.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IExperienceService _experienceService;
        private readonly int _userId;

        public PortfolioService(
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

        public async Task<ICollection<ResumeResponse>> GetResumesAsync()
        {
            var resumeModels = await _unitOfWork.Resume.Where(e => e.UserId == this._userId).ToListAsync();
            var topicModels = await this._unitOfWork.Topic.Where(e => e.UserId == this._userId).ToListAsync();
            var top_ExpModels = (from topic in topicModels
                                 join _ in await _unitOfWork.Topic_Experience.GetAll().ToListAsync() on topic.Id equals _.TopicId into groupjoin
                                 from _joinTP in groupjoin.DefaultIfEmpty()
                                 select new
                                 {
                                     TopicId = topic.Id,
                                     ExpId = _joinTP.ExperienceId,
                                 }).OrderBy(t => t.TopicId).ToList();

            var resumeResponses = _mapper.Map<List<ResumeResponse>>(resumeModels);
            var topicResponses = _mapper.Map<List<TopicResponse>>(topicModels);
            var allExpResponses = await this._experienceService.GetExperiencesAsync();

            foreach (var item in topicResponses)
            {
                var expIds = top_ExpModels.Where(te => te.TopicId == item.Id).Select(te => te.ExpId).ToList();
                item.Experiences = allExpResponses.Where(e => expIds.Contains(e.Id)).ToList();
            }


            foreach (var resume in resumeResponses)
            {

                resume.Topics = topicResponses.Where(t => t.ResumeId == resume.Id).ToList();
            }

            return resumeResponses;
        }

    }
}
