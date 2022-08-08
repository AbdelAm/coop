using coop2._0.Entities;
using coop2._0.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public interface IUserService
    {
        Task<ItemsModel<UserItemModel>> FindUsers(int page);
        Task<UserItemModel> FindUser(string cif);
        Task<IEnumerable<UserItemModel>> FindBy(string value);
        Task<bool> Validate(List<string> users, int page);
        Task<bool> Reject(List<string> users, int page);
        Task<bool> Delete(List<string> users, int page);
        Task<bool> ChangeInfo(UserInfoModel model);
        Task<Response> ChangeEmail(EmailUpdateModel model);
        Task<Response> ChangePassword(PasswordUpdateModel model);
    }
}