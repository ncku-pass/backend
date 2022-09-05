using Application.Dto.Data;
using Application.Dto.Responses;
using Application.Services.Interface;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class DefaultDataService : IDefaultDataService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IResumeService _resumeService;
        private readonly IExperienceService _experienceService;
        private readonly ITagService _tagService;
        private int _userId;

        public DefaultDataService(
            IHttpContextAccessor httpContextAccessor,
            IResumeService resumeService,
            IExperienceService experienceService,
            ITagService tagService
            )
        {
            this._httpContextAccessor = httpContextAccessor;
            this._resumeService = resumeService;
            this._experienceService = experienceService;
            this._tagService = tagService;
            try
            {
                this._userId = int.Parse(this._httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            }
            catch
            {
                this._userId = 1;
            }
        }

        public async Task<string> CreateGuideExampleAsync()
        {
            // 讀取範例資料檔
            string jsonString;
            try
            {
                string filePath = System.IO.Directory.GetCurrentDirectory();
                jsonString = File.ReadAllText(Path.Combine(filePath.Remove(filePath.Length - 3), @"Application\Dto\DefaultData\DefaultDataGuideExample.json"));
            }
            catch
            {
                jsonString = File.ReadAllText("/src/data/DefaultDataGuideExample.json");
            }
            DefaultDataGuideExample guideExample = JsonConvert.DeserializeObject<DefaultDataGuideExample>(jsonString)!;


            // 1.新增範例Tag
            var tagResponse = await this._tagService.AddTagAsync(guideExample.TagCreateMessages);

            // 2.新增範例Experience
            foreach (var exp in guideExample.ExperienceCreateMessages)
            {
                for (int i = 0; i < exp.Tags.Length; i++)
                {
                    // 原本exp.Tags存的是Tag在範例中的序號，這邊更換成實際tagId
                    exp.Tags[i] = tagResponse[exp.Tags[i]].Id;
                }
            }

            List<ExperienceResponse> expResponses = new List<ExperienceResponse> { };
            foreach (var exp in guideExample.ExperienceCreateMessages)
            {
                expResponses.Add(await this._experienceService.AddExperienceAsync(exp));
            }


            // 3.新增範例Resume
            foreach (var card in guideExample.ResumeSaveMessage.Cards)
            {
                for (int i = 0; i < card.Experiences.Count; i++)
                {
                    // 原本card.Experiences存的是Exp在範例中的序號，這邊更換成實際expId
                    card.Experiences[i].Id = expResponses[card.Experiences[i].Id].Id;
                }
            }

            await this._resumeService.SaveResumeAsync(guideExample.ResumeSaveMessage);

            return "succeeded";
        }
    }


}
