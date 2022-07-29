import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ForgetPasswordModel } from '../models/forget-password-model';
import { ItemsModel } from '../models/items-model';
import { LoginModel } from '../models/login-model';
import { RegisterModel } from '../models/register-model';
import { ResetPasswordModel } from '../models/reset-password-model';
import { TokenModel } from '../models/token-model';
import { UserItemModel } from '../models/user-item-model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {
  }
  getUsers(page: number = 0): Observable<ItemsModel<UserItemModel>> {
    return this.http.get<ItemsModel<UserItemModel>>(this.baseUrl + `user/list/${page}`);
  }
  validateUsers(userList: Array<string>, page: number = 1): Observable<UserItemModel[]> {
    return this.http.post<UserItemModel[]>(this.baseUrl + `user/validate/${page}`, userList);
  }
  rejectUsers(userList: Array<string>, page: number = 1): Observable<UserItemModel[]> {
    return this.http.post<UserItemModel[]>(this.baseUrl + `user/reject/${page}`, userList);
  }
  deleteUsers(userList: Array<string>, page: number = 1): Observable<UserItemModel[]> {
    return this.http.post<UserItemModel[]>(this.baseUrl + `user/delete/${page}`, userList);
  }
}
