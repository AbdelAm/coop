import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ForgetPasswordModel } from '../models/forget-password-model';
import { LoginModel } from '../models/login-model';
import { RegisterModel } from '../models/register-model';
import { ResetPasswordModel } from '../models/reset-password-model';
import { TokenModel } from '../models/token-model';
import { UserModel } from '../models/user-model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  readonly baseUrl = environment.apiUrl;
  user: UserModel;

  constructor(private http: HttpClient) {
    this.user = new UserModel();
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
  getUsers(page: number = 1): Observable<UserModel[]> {
    return this.http.get<UserModel[]>(this.baseUrl + `user/list/${page}`);
  }
  validateUsers(userList: Array<number>): Observable<UserModel[]> {
    return this.http.post<UserModel[]>(this.baseUrl + 'user/validate', userList);
  }
  deleteUsers(userList: Array<number>): Observable<UserModel[]> {
    return this.http.post<UserModel[]>(this.baseUrl + 'user/delete', userList);
  }
}
