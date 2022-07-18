using coop2._0.Entities;
using coop2._0.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace coop2._0.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;

        public RequestService(IRequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
        }


        public Task<ActionResult<Request>> GetRequest(int id)
        {
            return _requestRepository.GetRequest(id);
        }

        public Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return _requestRepository.GetRequests();
        }
    }
}
