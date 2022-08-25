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
        private const int PageSize = 5;


        public RequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<object> GetRequests(PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var response = await _context.Requests
                .OrderByDescending(r => r.DateRequest)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

            var pagination = new PaginationResponse(validFilter.PageNumber, validFilter.PageSize,
                await _context.Requests.CountAsync());

            return new { response, pagination };
        }
        public async Task<object> FilterRequests(Status status, PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            var response = await _context.Requests
                .OrderByDescending(r => r.DateRequest)
                .Where(r => r.Status == status)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();

            var totalRecords = await _context.Requests.CountAsync(r => r.Status == status);
            var pagination = new PaginationResponse(validFilter.PageNumber, validFilter.PageSize,
                totalRecords);

            return new { response, pagination };
        }



        public async Task<Request> GetRequest(int id)
        {
            return await _context.Requests.Include(req => req.User).Where(req => req.Id == id).FirstOrDefaultAsync();
        }

        public async Task<object> GetRequestsByUser(string userId, PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var response = await _context.Requests.Where(b => b.UserId == userId)
                .OrderByDescending(r => r.DateRequest)
                .Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize)
                .ToListAsync();
            var totalRecords = await _context.Requests.CountAsync(b => b.UserId == userId);
            var pagination = new PaginationResponse(validFilter.PageNumber, validFilter.PageSize,
                totalRecords);

            return new { response, pagination };
        }

        public async Task<IEnumerable<Request>> SelectByUser(string userId)
        {
            return await _context.Requests.Where(b => b.UserId == userId).ToListAsync();
        }

        public async Task<bool> RemoveRequest(Request request)
        {
            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Request> RejectRequest(Request request)
        {
            _context.Entry(request).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return request;
        }

        public async Task<ActionResult<Request>> AddRequest(RequestModel model)
        {
            var request = new Request()
            {
                Message = model.Message,
                Type = model.Type,
                UserId = model.UserId,
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
        public async Task<IEnumerable<RequestModel>> SelectAll(int page)
        {
            return await _context.Requests
                .OrderByDescending(u => u.DateRequest)
                .Skip(page * PageSize)
                .Take(PageSize)
                .Select(u => new RequestModel())
                .ToListAsync();
        }
        public async Task<int> SelectProgressCount()
        {
            return await _context.Requests.Where(r => r.Status == Status.Progress)
                .CountAsync();
        }
    }
}