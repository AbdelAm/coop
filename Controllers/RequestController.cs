using coop2._0.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace coop2._0.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public void MakeRequest(Request request)
        {
            if (ModelState.IsValid)
            {
                request.Status = Status.Progress;
                _context.Requests.Add(request);
                _context.SaveChanges();
            }
        }

        public void ValidateRequest(Request request)
        {
            if (ModelState.IsValid)
            {
                request.Status = Status.Approuved;
                _context.SaveChanges();
            }
        }

        public void RejectRequest(Request request)
        {
            if (ModelState.IsValid)
            {
                request.Status = Status.Rejected;
                _context.SaveChanges();
            }
        }
    }
}