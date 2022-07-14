using coop2._0.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace coop2._0.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        ApplicationDbContext Context = new ApplicationDbContext();

        public void MakRequest(Request r)
        {
            if(ModelState.IsValid)
            {
                r.Status = Status.Progress;
                Context.Requests.Add(r);
                Context.SaveChanges();
            }
        }
        public void ValidateRequest(Request r)
        {
            if (ModelState.IsValid)
            {
                r.Status = Status.Approuved;
                Context.SaveChanges();
            }
        }
        public void RejecteRequest(Request r)
        {
            if (ModelState.IsValid)
            {
                r.Status = Status.Rejected;
                Context.SaveChanges();
            }
            
        }


    }
}
