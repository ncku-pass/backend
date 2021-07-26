using System.Threading.Tasks;

namespace Infrastructure.Service.Interface
{
    public interface INCKUPortalAPI
    {
        Task<string> GetExpRecordAsync(string key, string keyval, string type);
    }
}