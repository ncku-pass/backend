using System.Threading.Tasks;

namespace Infrastructure.Services.Interface
{
    public interface INCKUPortalAPI
    {
        Task<string> GetExpRecordAsync(string key, string keyval, string type);
    }
}