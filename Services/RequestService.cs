using coop2._0.Entities;
using coop2._0.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace coop2._0.Services
{
    public class RequestService : IRequestService
    {
        private IRequestRepository _requestnRepository;

        public RequestService(IRequestRepository requestnRepository)
        {
            _requestnRepository = requestnRepository;
        }


        public Task<ActionResult<Request>> GetRequest(int id)
        {
            return _requestnRepository.GetRequest(id);
        }

        public Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return _requestnRepository.GetRequests();
        }
    }
}
