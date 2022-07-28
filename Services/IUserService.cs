using coop2._0.Entities;
using coop2._0.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public interface IUserService
    {
        Task<UserItems> GetAll(int page);
        Task<bool> Validate(List<string> users, int page);
        Task<bool> Reject(List<string> users, int page);
        Task<bool> Delete(List<string> users, int page);
    }
}