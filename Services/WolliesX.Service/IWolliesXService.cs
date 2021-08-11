using System.Collections.Generic;
using System.Threading.Tasks;
using WolliesX.Service.Models;
using WolliesX.Service.Models.v1;

namespace WolliesX.Service
{
    public interface IWolliesXService
    {
        Task<Result<string>> GetResource();
        Task<Result<IEnumerable<Product>>> GetSortedProducts(string sortOption);
        Task<Result<double>> GetTrolleyTotal(WolliesX.Service.Models.v1.Trolley.Trolley trolley);
    }
}
