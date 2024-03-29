﻿using coop2._0.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace coop2._0.Services
{
    public interface IUserService
    {
        Task<ItemsModel<UserItemModel>> FindUsers(int page);
        Task<UserItemModel> FindUser(string cif);
        Task<UserBankItemModel> FindUserWithBank(string cif);
        Task<IEnumerable<UserItemModel>> FindBy(string value);
        Task<Response> Validate(List<string> users);
        Task<Response> Reject(List<string> users);
        Task<Response> Delete(List<string> users);
        Task<Response> ChangeInfo(UserInfoModel model);
        Task<Response> ChangeEmail(EmailUpdateModel model);
        Task<Response> ChangePassword(PasswordUpdateModel model);
    }
}