using coop2._0.Entities;
using coop2._0.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Repositories
{
    public interface IRequestRepository
    {
        Task<ActionResult<IEnumerable<Request>>> GetRequests();
        Task<ActionResult<Request>> GetRequest(int id);
        Task<ActionResult> RemoveRequest(int id);
        Task<ActionResult<Request>> RejectRequest(int id);
        Task<ActionResult<Request>> ValidateRequest(int id);
        Task<ActionResult<Request>> AddRequest(RequestModel model);
    }
}
