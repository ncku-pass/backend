using Application.Dto;
using Application.Dto.Messages;
using Application.Services.Interface;
using AutoMapper;
using Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
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


        public Task<TagResponse> AddTagAsync(ICollection<TagCreateMessage> tags)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteTagAsync(int tagId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TagResponse>> GetExperienceTagsAsync(int experienceId)
        {
            throw new NotImplementedException();
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
