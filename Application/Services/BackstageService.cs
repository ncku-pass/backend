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
        private readonly IExperienceService _experienceService;

        public BackstageService(
            IUnitOfWork unitOfWork,
            IExperienceService experienceService
            )
        {
            this._unitOfWork = unitOfWork;
            this._experienceService = experienceService;
        }


        public async Task<BackstageCategoriesAnalyzeResponse> CategoriesAnalyze(BackstageCategoriesAnalyzeMessage message)
        {
            // 1. 篩選出DepIds
            var depModel = await this._unitOfWork.Department.ToListAsync();
            var depIds = from d in depModel
                        where (message.Colleges.Master.Contains(d.College) & d.Degree == "master") ||
                              (message.Colleges.Bachelor.Contains(d.College) & d.Degree == "bachelor") ||
                              (message.Departments.Contains(d.Prefix))
                        select d.Id;


            // 2. 從UserTable篩出在DepIds中的使用者
            var userIds = await this._unitOfWork.User.Where(u => depIds.Contains(u.DepartmentId))
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
            BackstageCategoriesAnalyzeResponse.Experiences = BackstageCategoriesAnalyzeResponse.Experiences.OrderByDescending(e => e.Count).ToList();
            BackstageCategoriesAnalyzeResponse.Tags = BackstageCategoriesAnalyzeResponse.Tags.OrderByDescending(t => t.Count).ToList();

            return BackstageCategoriesAnalyzeResponse;
        }
    }
}
