using coop2._0.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Repositories
{
    public interface IRequestRepository
    {
        Task<ActionResult<IEnumerable<Request>>> GetRequests();
        Task<ActionResult<Request>> GetRequest(int id);
    }
}
