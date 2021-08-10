using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WolliesX.Service;

namespace WooliesX.David.Api.Controllers
{
    [Route("api/david")]
    [ApiController]
    public class DavidController : ControllerBase
    {

        private readonly IWolliesXService _wolliesXService;

        public DavidController(IWolliesXService wolliesXService)
        {
            _wolliesXService = wolliesXService;
        }

        [HttpGet("")]
        public async Task<ActionResult<string>> GetByIdAsync(string id)
        {
            var response = await _wolliesXService.GetResource();
            return response;
        }

        //[HttpPost("")]
        //public async Task<IActionResult> PostAsync([FromBody] Dto.v2.Approval approvalDto)
        //{
        //    if (!this.ModelState.IsValid)
        //        return this.BadRequest(this.ModelState);

        //    var approval = _mapper.Map<Library.Entities.Approval>(approvalDto);
        //    _approvalService = _approvalServiceFactory.GetService(approval.ApprovalType.Value);

        //    approval = await _approvalService.CreateApprovalAsync(approval);
        //    approvalDto = _mapper.Map<Dto.v2.Approval>(approval);
        //    return CreatedAtAction(nameof(this.GetByIdAsync), new { id = approval.Id }, approvalDto);
        //}

    }
}