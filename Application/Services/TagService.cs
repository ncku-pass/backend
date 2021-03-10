using Application.Dto.Messages;
using Application.Dto.Responses;
using Application.Services.Interface;
using AutoMapper;
using Infrastructure.Infrastructure;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TagService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<bool> SaveAsync()
        {
            return await _unitOfWork.SaveChangeAsync();
        }

        public Tag AddTagAsync(TagCreateMessage tags)
        {
            var tagModel = this._mapper.Map<Tag>(tags);
            tagModel.UserId = 1;
            this._unitOfWork.Tag.Add(tagModel);
            return tagModel;
        }

        public async void DeleteTagAsync(int tagId)
        {
            var tagModel = await _unitOfWork.Tag.FirstOrDefaultAsync(t => t.Id == tagId);
            this._unitOfWork.Tag.Remove(tagModel);
        }

        public async Task<IEnumerable<TagResponse>> GetExperienceTagsAsync(int experienceId)
        {
            var tagsResponse = await (from tag in _unitOfWork.Tag.GetAll()
                                      join combine in _unitOfWork.Tag_Experience.Where(t => t.ExperienceId == experienceId)
                                          on tag.Id equals combine.TagId
                                      select new TagResponse()
                                      {
                                          Id = tag.Id,
                                          Name = tag.Name
                                      }).ToListAsync();
            return tagsResponse;
        }

        public Task<TagResponse> GetTagByIdAsync(int tagId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TagExistsAsync(int tagId)
        {
            throw new NotImplementedException();
        }
    }
}