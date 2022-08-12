using coop2._0.Entities;
using coop2._0.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public interface IRequestService
    {
        Task<Request> GetRequest(int id);
        Task<object> GetRequests(PaginationFilter filter);
        Task<object> GetRequestsByUser(string userId, PaginationFilter filter);
        Task<bool> RemoveRequest(List<int> requests);
        Task<bool> RejectRequest(List<int> requests);
        Task<bool> ValidateRequest(List<int> requests);
        Task<ActionResult<Request>> AddRequest(RequestModel model);
    }
}