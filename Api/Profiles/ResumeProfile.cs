﻿using Api.RequestModel.Parameters;
using Api.RequestModel.ViewModels;
using Application.Dto.Messages;
using Application.Dto.Responses;
using AutoMapper;
using Infrastructure.Models;
using System;

namespace Api.Profiles
{
    public class ResumeProfile : Profile
    {
        public ResumeProfile()
        {
            CreateMap<Card, CardResponse>()
            .ForMember(
                dest => dest.Type,
                opt => opt.MapFrom(src => src.Type.ToString())
            );
            CreateMap<CardResponse, CardViewModel>();

            CreateMap<Resume, ResumeResponse>();
            CreateMap<ResumeResponse, ResumeViewModel>();

            CreateMap<ResumeSaveParameter, ResumeSaveMessage>();
            CreateMap<ResumeSaveMessage, Resume>();
            CreateMap<CardSaveParameter, CardSaveMessage>();
            CreateMap<CardSaveMessage, Card>();

            CreateMap<ExpInCardParameter, ExpInCardMessage>();
            CreateMap<ExpInCardMessage, Card_Experience>();

            CreateMap<Card_Experience, ExpInCardResponse>();
            CreateMap<ExperienceResponse, ExpInCardResponse>();
            CreateMap<ExpInCardResponse, ExpInCardViewModel>()
            .ForMember(
                dest => dest.Categories,
                opt => opt.MapFrom(src => src.Categories.Split(new char[] { ' ', ';' }, StringSplitOptions.RemoveEmptyEntries))
            );
        }
    }
}