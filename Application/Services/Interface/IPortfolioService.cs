using Application.Dto.Messages;
using Application.Dto.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IPortfolioService
    {
        Task<ICollection<ResumeResponse>> GetResumesAsync();
        Task<ResumeResponse> SaveResumesAsync(PortfolioSaveMessage portfolioSaveMessage);
    }
}
