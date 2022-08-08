using coop2._0.Entities;
using coop2._0.Model;
using coop2._0.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        // context is in repository implementation, remove it from controller
        // every method that interacts with the database should be in the repository implementation 

        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpGet]
        [Route("list/{page}")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests(int page)
        {
            return await _requestService.GetRequests();
        }

        [HttpPost("list/{id}/{page}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _requestService.GetRequest(id);
            if (request == null)
                return NotFound();
            return Ok(request);
        }

        [HttpPost("delete")]
        public async Task<ActionResult> RemoveRequest([FromBody] List<int> requests)
        {
            try
            {
                var deletedRequest = await _requestService.RemoveRequest(requests);
                return Ok(deletedRequest);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reject")]
        public async Task<ActionResult> RejectRequest([FromBody] List<int> requests)
        {
            try
            {
                var res = await _requestService.RejectRequest(requests);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("validate")]
        public async Task<ActionResult> ValidateRequest([FromBody] List<int> requests)
        {
            try
            {
                var res = await _requestService.ValidateRequest(requests);
                return Ok(res);
            } catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("add")]
        public async Task<ActionResult<Request>> AddRequest(RequestModel model)
        {
            return await _requestService.AddRequest(model);
        }

 
    }
}