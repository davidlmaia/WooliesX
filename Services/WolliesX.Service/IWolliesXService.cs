using System.Threading.Tasks;

namespace WolliesX.Service
{
    public interface IWolliesXService
    {
        Task<string> GetResource();
    }
}
