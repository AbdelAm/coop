import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LoginModel } from '../models/login-model';
import { RegisterModel } from '../models/register-model';
import { TokenModel } from '../models/token-model';
import { UserModel } from '../models/user-model';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  readonly baseUrl = environment.apiUrl;
  user: UserModel;
  registerModel : RegisterModel;
  loginModel: LoginModel;

  constructor(private http: HttpClient) {
    this.user = new UserModel();
    this.registerModel = new RegisterModel();
    this.loginModel = new LoginModel();
  }

  register(): Observable<Response> {
    return this.http.post<Response>(this.baseUrl + '/register', this.registerModel);
  }
  login(): Observable<TokenModel> {
    return this.http.post<TokenModel>(this.baseUrl + '/login', this.loginModel);
  }
  getUsers(page: number = 1): Observable<UserModel[]> {
    return this.http.get<UserModel[]>(this.baseUrl + `/admin/users/list/${page}`);
  }
  validateUsers(userList: Array<number>): Observable<UserModel[]> {
    return this.http.post<UserModel[]>(this.baseUrl + '/admin/users/validate', userList);
  }
  deleteUsers(userList: Array<number>): Observable<UserModel[]> {
    return this.http.post<UserModel[]>(this.baseUrl + '/admin/users/delete', userList);
  }
}
