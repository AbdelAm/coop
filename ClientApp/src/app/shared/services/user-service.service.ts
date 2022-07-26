import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ForgetPasswordModel } from '../models/forget-password-model';
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

  register(registerModel: RegisterModel): Observable<Response> {
    return this.http.post<Response>(this.baseUrl + 'user/register', registerModel);
  }
  confirmEmail(url: string): Observable<Response>
  {
    return this.http.get<Response>(this.baseUrl + `user${url}`);
  }
  login(loginModel: LoginModel): Observable<TokenModel> {
    return this.http.post<TokenModel>(this.baseUrl + 'user/login', loginModel);
  }
  forgetPassword(forgetModel: ForgetPasswordModel): Observable<Response>
  {
    return this.http.post<Response>(this.baseUrl + 'user/forget-password', forgetModel);
  }
  resetPassword(resetModel: ResetPasswordModel): Observable<Response>
  {
    return this.http.post<Response>(this.baseUrl + 'user/reset-password', resetModel);
  }
  getUsers(page: number = 1): Observable<UserItemModel[]> {
    return this.http.get<UserItemModel[]>(this.baseUrl + `user/list/${page}`);
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
