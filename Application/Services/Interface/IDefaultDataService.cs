using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interface
{
    public interface IDefaultDataService
    {
        Task<string> CreateGuideExampleAsync();
    }
}
