using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WolliesX.Service;
using WolliesX.Service.Models.v1;

namespace WooliesX.David.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController()
        {
        }

        [HttpGet("")]
        public async Task<ActionResult<User>> GetUserAsync()
        {
            return new User();
        }
    }
}