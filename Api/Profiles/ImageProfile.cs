using Application.Dto.Responses;
using AutoMapper;
using Infrastructure.Models;

namespace Api.Profiles
{
    public class ImageProfile : Profile
    {
        public ImageProfile()
        {
            CreateMap<Image, ImageResponse>()
                .ForMember(
                    dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name + "." + src.Extension)
                );
        }
    }
}
