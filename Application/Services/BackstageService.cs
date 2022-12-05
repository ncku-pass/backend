using Application.Dto.Messages;
using Application.Dto.Responses;
using Application.Services.Interface;
using Infrastructure.Infrastructure;
using Infrastructure.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BackstageService : IBackstageService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BackstageService(
            IUnitOfWork unitOfWork
            )
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task<BackstageCategoriesAnalyzeResponse> CategoriesAnalyze(BackstageCategoriesAnalyzeMessage message)
        {
            // 1. 篩選出DepIds
            var depModel = await this._unitOfWork.Department.ToListAsync();
            var depIds = from d in depModel
                         where (message.Colleges.Master.Contains(d.College) && d.Degree == "master") ||
                               (message.Colleges.Bachelor.Contains(d.College) && d.Degree == "bachelor") ||
                               (message.Departments.Contains(d.Prefix))
                         select d.Id;

            // 2. 從UserTable篩出在DepIds中的使用者
            var userIds = await this._unitOfWork.User.Where(u => u.EnrollmentYear >= message.YearStart && u.EnrollmentYear <= message.YearEnd)
                                                     .Where(u => depIds.Contains(u.DepartmentId))
                                                     .Select(u => u.Id)
                                                     .ToListAsync();

            // 3. 從ExpTable篩出這些使用者指定類別的ExpModel

            // 指定類別轉為Enum序號
            List<ExperienceType> catagories = new List<ExperienceType>();
            foreach (var item in message.Categories)
            {
                var temp = (ExperienceType)Enum.Parse(typeof(ExperienceType), item, true);
                catagories.Add(temp);
            }

            var expModels = await this._unitOfWork.Experience.Where(e => userIds.Contains(e.UserId) && !e.Name.Contains("【範例】") && catagories.Contains(e.Type)).ToListAsync();
            var usedTagNames = await (from combine in this._unitOfWork.Experience_Tag.Where(et => expModels.Select(e => e.Id).Contains(et.ExperienceId))
                                      join tag in this._unitOfWork.Tag.Where(t => !t.Name.Contains("【範例】"))
                                         on combine.TagId equals tag.Id
                                      select tag.Name).ToListAsync();

            // 4. 用字典檔整理ExpName、TagName
            Dictionary<string, int> expResult = new Dictionary<string, int>();
            Dictionary<string, int> tagResult = new Dictionary<string, int>();

            foreach (var exp in expModels)
            {
                if (!expResult.ContainsKey(exp.Name))
                {
                    expResult.Add(exp.Name, 1);
                }
                else
                {
                    expResult[exp.Name] += 1;
                }
            }

            foreach (var tagName in usedTagNames)
            {
                if (!tagResult.ContainsKey(tagName))
                {
                    tagResult.Add(tagName, 1);
                }
                else
                {
                    tagResult[tagName] += 1;
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