using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Dto.Messages;
using Application.Dto.Responses;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Profiles
{
    public class BackstageProfile : Profile
    {
        public BackstageProfile()
        {
            CreateMap<BackstageCategoriesAnalyzeParameter, BackstageCategoriesAnalyzeMessage>();
            CreateMap<BackstageCategoriesAnalyzeResponse, BackstageCategoriesAnalyzeViewModel>();

            CreateMap<BackstageCategoriesAnalyzeResponseItem, BackstageCategoriesAnalyzeViewModelItem>();
        }
    }
}
