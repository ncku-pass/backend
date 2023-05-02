using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Dto.Messages;
using Application.Dto.Responses;
using AutoMapper;

namespace Api.Profiles
{
    public class BackstageProfile : Profile
    {
        public BackstageProfile()
        {
            CreateMap<BackstageCategoriesAnalyzeParameter, BackstageCategoriesAnalyzeMessage>();
            CreateMap<BackstageCategoriesAnalyzeCollegesParameter, BackstageCategoriesAnalyzeCollegesMessage>();

            CreateMap<BackstageCategoriesAnalyzeResponse, BackstageCategoriesAnalyzeViewModel>();
            CreateMap<BackstageCategoriesAnalyzeResponseItem, BackstageCategoriesAnalyzeViewModelItem>();

            CreateMap<BackStageAbilityAnalyzeResponse, BackStageAbilityAnalyzeViewModel>();
            CreateMap<BackStageAbilityAnalyzeTagResponseItem, BackStageAbilityAnalyzeTagViewModelItem>();
            CreateMap<BackStageAbilityAnalyzeExpResponseItem, BackStageAbilityAnalyzeExpViewModelItem>();
        }
    }
}