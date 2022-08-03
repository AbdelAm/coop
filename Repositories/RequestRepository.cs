using coop2._0.Controllers;
using coop2._0.Entities;
using coop2._0.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coop2._0.Repositories
{
    public class RequestRepository : IRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public RequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return await _context.Requests.ToListAsync();
        }


        public async Task<Request> GetRequest(int id)
        {
            return await _context.Requests.Include(req => req.User).Where(req => req.Id == id).FirstOrDefaultAsync();
        }
        public async Task<ActionResult> RemoveRequest(List<int> requests)
        {
            var request = await _context.Requests.FindAsync(requests);

            if (request == null || request.Status == Status.Approuved) return null;

            _context.Requests.Remove(request);

            await _context.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<ActionResult<Request>> RejectRequest(List<int> requests)
        {
            var request = await _context.Requests.FindAsync(requests);

            if (request is not { Status: Status.Progress }) return null;
            request.Status = Status.Rejected;
            _context.Update(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<ActionResult<Request>> AddRequest(RequestModel model)
        {
            
            var request = new Request()
            {
                Id = model.Id,
                Message = model.Message,
                Type = model.Type,
                DateRequest = DateTime.Now,
                Status = model.Status
            };
            _context.Requests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<Request> ValidateRequest(Request request)
        {
            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return request;
        }

    }
}
