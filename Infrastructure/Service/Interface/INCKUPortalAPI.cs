using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service.Interface
{
    public interface INCKUPortalAPI
    {
        Task<string> GetExpRecordAsync(string key, string keyval, string type);
    }
}
