using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WolliesX.Service;
using WolliesX.Service.Models;
using WolliesX.Service.Models.v1;

namespace WooliesX.David.Api.Controllers
{
    [Route("api/answers")]
    [ApiController]
    public class WooliesXTestController : ControllerBase
    {

        private readonly IWolliesXService _wolliesXService;

        public WooliesXTestController(IWolliesXService wolliesXService)
        {
            _wolliesXService = wolliesXService;
        }

        [HttpGet]
        [Route("Sort")]
        public async Task<IEnumerable<Product>> Sort([FromQuery] string sortOption)
        {
            var response = await _wolliesXService.GetSortedProducts(sortOption);
            if (!response.Success)
            {
                return null;
            }
            return response.Value;
        }

        [HttpPost]
        [Route("TrolleyTotal")]
        public async Task<double> GetTrolleyTotalAsync([FromBody] WolliesX.Service.Models.v1.Trolley.Trolley trolley)
        {
            var trolleyTotal = await _wolliesXService.GetTrolleyTotal(trolley);
            return trolleyTotal.Value;
        }

    }
}