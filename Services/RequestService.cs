using coop2._0.Entities;
using coop2._0.Model;
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


        public Task<Request> GetRequest(int id)
        {
            return _requestRepository.GetRequest(id);
        }

        public Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
            return _requestRepository.GetRequests();
        }
        public Task<ActionResult> RemoveRequest(List<int> requests)
        {
            return _requestRepository.RemoveRequest(requests);
        }

        public async Task<ActionResult<Request>> RejectRequest(List<int> requests)
        {
            return await _requestRepository.RejectRequest(requests);
        }

        public async Task<ActionResult<Request>> AddRequest(RequestModel model)
        {
            return await _requestRepository.AddRequest(model);
        }

        public async Task<bool> ValidateRequest(List<int> requests)
        {
            bool temoin = true;
            foreach(int id in requests)
            {
                var request = await _requestRepository.GetRequest(id);
                request.Status = Status.Approuved;
                var result = await _requestRepository.ValidateRequest(request);
                if(result == null)
                {
                    temoin = false;
                }

            }
            if(!temoin)
            {
                throw new System.Exception("some requests doesn't approuved, please try again later");
            }
            return temoin;
        }
    }
}
