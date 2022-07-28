using coop2._0.Entities;
using coop2._0.Model;
using coop2._0.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        private ApplicationDbContext _context;

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

        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var request = await _requestService.GetRequest(id);
            if (request == null)
                return NotFound();
            return Ok(request);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> RemoveRequest(int id)
        {
            var deletedRequest = await _requestService.RemoveRequest(id);
            if (deletedRequest == null)
                return BadRequest();
            return Ok(deletedRequest);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> RejectRequest(int id)
        {
            return await _requestService.RejectRequest(id);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> ValidateRequest(int id)
        {
            return await _requestService.ValidateRequest(id);
        }

        [HttpPost]
        public async Task<ActionResult<Request>> AddRequest(RequestModel model)
        {
            return await _requestService.AddRequest(model);
        }

 
    }
}