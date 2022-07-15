using coop2._0.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace coop2._0.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        public readonly ApplicationDbContext _context;

        public RequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public void MakRequest(Request r)
        {
            if(ModelState.IsValid)
            {
                r.Status = Status.Progress;
                _context.Requests.Add(r);
                _context.SaveChanges();
            }
        }
        public void ValidateRequest(Request r)
        {
            if (ModelState.IsValid)
            {
                r.Status = Status.Approuved;
                _context.SaveChanges();
            }
        }
        public void RejecteRequest(Request r)
        {
            if (ModelState.IsValid)
            {
                r.Status = Status.Rejected;
                _context.SaveChanges();
            }
            
        }


    }
}
