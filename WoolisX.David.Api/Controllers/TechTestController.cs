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
    public class TechTestController : ControllerBase
    {

        private readonly IWolliesXService _wolliesXService;

        public TechTestController(IWolliesXService wolliesXService)
        {
            _wolliesXService = wolliesXService;
        }

        [HttpGet]
        public async Task<Result<IEnumerable<Product>>> Sort([FromQuery] string sortOption)
        {
            var response = await _wolliesXService.GetSortedProducts(sortOption);
            return response;
        }

        [HttpPost]
        [Route("TrolleyTotal")]
        public async Task<Result<double>> GetTrolleyTotalAsync([FromBody] WolliesX.Service.Models.v1.Trolley.Trolley trolley)
        {
            var trolleyTotal = await _wolliesXService.GetTrolleyTotal(trolley);
            return trolleyTotal;
        }

    }
}