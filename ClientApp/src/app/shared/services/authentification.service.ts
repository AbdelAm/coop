import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ForgetPasswordModel } from '../models/forget-password-model';
import { LoginModel } from '../models/login-model';
import { RegisterModel } from '../models/register-model';
import { ResetPasswordModel } from '../models/reset-password-model';
import { TokenModel } from '../models/token-model';

@Injectable({
  providedIn: 'root'
})
export class AuthentificationService {

  readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {
  }

  register(registerModel: RegisterModel): Observable<Response> {
    return this.http.post<Response>(this.baseUrl + 'authentification/register', registerModel, {headers:{skip:"true"}});
  }
  registerAdmin(registerModel: RegisterModel): Observable<Response> {
    return this.http.post<Response>(this.baseUrl + 'authentification/administrateur/register', registerModel, {headers:{skip:"true"}});
  }
  confirmEmail(url: string): Observable<Response>
  {
    return this.http.get<Response>(this.baseUrl + `authentification${url}`, {headers:{skip:"true"}});
  }
  login(loginModel: LoginModel): Observable<TokenModel> {
    return this.http.post<TokenModel>(this.baseUrl + 'authentification/login', loginModel, {headers:{skip:"true"}});
  }
  forgetPassword(forgetModel: ForgetPasswordModel): Observable<Response>
  {
    return this.http.post<Response>(this.baseUrl + 'authentification/forget-password', forgetModel, {headers:{skip:"true"}});
  }
  resetPassword(resetModel: ResetPasswordModel): Observable<Response>
  {
    return this.http.post<Response>(this.baseUrl + 'authentification/reset-password', resetModel, {headers:{skip:"true"}});
  }
}
