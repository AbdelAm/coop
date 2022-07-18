using coop2._0.Entities;
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
        private  ApplicationDbContext _context;
        IRequestService _RequestService;

        
        public RequestController(IRequestService RequestService)
        {
            _RequestService = RequestService;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _RequestService.GetRequests();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Request>> GetRequest(int id)
        {
            var transaction = await _RequestService.GetRequest(id);
            if (transaction == null)
                return NotFound();
            return Ok(transaction);
        }
        public void MakeRequest(Request request)
        {
            if (ModelState.IsValid)
            {
                request.Status = Status.Progress;
                _context.Requests.Add(request);
            }
        }

        public void ValidateRequest(int id)
        {
            if (ModelState.IsValid )
            {
                GetRequest(id).Status=Status.Approuved;
            }
        }

        public void RejectRequest(int id)
        {
            if (ModelState.IsValid)
            {
                GetRequest(id).Status = Status.Rejected;
            }
        }
    }
    
}