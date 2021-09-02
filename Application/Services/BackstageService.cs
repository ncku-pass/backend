using Application.Dto.Messages;
using Application.Dto.Responses;
using Application.Services.Interface;
using AutoMapper;
using Infrastructure.Infrastructure;
using Infrastructure.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BackstageService : IBackstageService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IExperienceService _experienceService;
        private readonly ITagService _tagService;
        private readonly IDepartmentService _departmentService;

        public BackstageService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IExperienceService experienceService,
            ITagService tagService,
            IDepartmentService departmentService
            )
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
            this._experienceService = experienceService;
            this._tagService = tagService;
            this._departmentService = departmentService;
        }


        public async Task<BackstageCategoriesAnalyzeResponse> CategoriesAnalyze(BackstageCategoriesAnalyzeMessage message)
        {
            // 1. 篩選出DepIds
            var DepIds = new List<int>();
            foreach (var item in message.Departments)
            {
                DepIds.Add(await this._departmentService.GetIdsByDepartment(item));
            }
            foreach (var item in message.Colleges)
            {
                DepIds = DepIds.Union(await this._departmentService.GetIdsByCollege(item)).ToList();
            }
            foreach (var item in message.Degrees)
            {
                DepIds = DepIds.Union(await this._departmentService.GetIdByDegree(item)).ToList();
            }

            // 2. 從UserTable篩出在DepIds中的使用者
            var userIds = await this._unitOfWork.User.Where(u => DepIds.Contains(u.DepartmentId))
                                                     .Select(u => u.Id)
                                                     .ToListAsync();

            // 3. 從ExpTable篩出這些使用者的ExpResponse、在篩除指定類別
            var expResponse = new List<ExperienceResponse>();
            foreach (var id in userIds)
            {
                expResponse.AddRange(await this._experienceService.GetByUserIdAsync(id));
            }
            var expCategoriesFilterResponse = new List<ExperienceResponse>();
            foreach (var item in message.Categories)
            {
                expCategoriesFilterResponse.AddRange(expResponse.Where(e => e.Type == item));
            }


            // 4. 用字典檔整理ExpName、TagName
            Dictionary<string, int> expResult = new Dictionary<string, int>();
            Dictionary<string, int> tagResult = new Dictionary<string, int>();

            foreach (var exp in expCategoriesFilterResponse)
            {
                if (!expResult.ContainsKey(exp.Name))
                {
                    expResult.Add(exp.Name, 1);
                }
                else
                {
                    expResult[exp.Name] += 1;
                }
                foreach (var tag in exp.Tags)
                {
                    if (!tagResult.ContainsKey(tag.Name))
                    {
                        tagResult.Add(tag.Name, 1);
                    }
                    else
                    {
                        tagResult[tag.Name] += 1;
                    }
                }
            }

            // 5. 整理字典檔結果輸出
            var BackstageCategoriesAnalyzeResponse = new BackstageCategoriesAnalyzeResponse();
            BackstageCategoriesAnalyzeResponse.Experiences = new List<BackstageCategoriesAnalyzeResponseItem>();
            BackstageCategoriesAnalyzeResponse.Tags = new List<BackstageCategoriesAnalyzeResponseItem>();

            foreach (var exp in expResult)
            {
                BackstageCategoriesAnalyzeResponse.Experiences.Add(new BackstageCategoriesAnalyzeResponseItem() { Name = exp.Key, Count = exp.Value });
            }
            foreach (var tag in tagResult)
            {
                BackstageCategoriesAnalyzeResponse.Tags.Add(new BackstageCategoriesAnalyzeResponseItem() { Name = tag.Key, Count = tag.Value });
            }

            return BackstageCategoriesAnalyzeResponse;
        }
    }
}
