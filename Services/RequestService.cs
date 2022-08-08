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
        public async Task<bool> RemoveRequest(List<int> requests)
        {
            bool temoin = true;

            foreach (int id in requests)
            {
                var request = await _requestRepository.GetRequest(id);
                var result = await _requestRepository.RemoveRequest(request);
                if (!result)
                {
                    temoin = false;
                }

            }
            if (!temoin)
            {
                throw new System.Exception("can't delete");
            }
            return temoin;
        }

        public async Task<bool> RejectRequest(List<int> requests)
        {
            bool temoin = true;
            foreach (int id in requests)
            {
                var request = await _requestRepository.GetRequest(id);
                request.Status = Status.Rejected;
                var result = await _requestRepository.RejectRequest(request);
                if (result == null)
                {
                    temoin = false;
                }

            }
            if (!temoin)
            {
                throw new System.Exception("some requests doesn't approuved, please try again later");
            }
            return temoin;
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
