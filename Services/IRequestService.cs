using coop2._0.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public interface IRequestService
    {
        Task<ActionResult<Request>> GetRequest(int id);
        Task<ActionResult<IEnumerable<Request>>> GetRequests();
    }
}
