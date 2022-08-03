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
        Task<Request> GetRequest(int id);
        Task<ActionResult> RemoveRequest(List<int> requests);
        Task<ActionResult<Request>> RejectRequest(List<int> requests);
        Task<Request> ValidateRequest(Request request);
        Task<ActionResult<Request>> AddRequest(RequestModel model);
    }
}
