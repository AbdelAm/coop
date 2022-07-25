import { HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TokenModel } from '../models/token-model';

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  jwt!: TokenModel;
  switchBtn!: boolean;

  constructor() {
    let res = localStorage.getItem('auth');
    if(res) this.jwt = JSON.parse(res);
    this.switchBtn = true;
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if(this.jwt){
            req = req.clone({
            headers: req.headers.set('Authorisation', `${this.jwt.token}`)});
    }
    return next.handle(req);
  }

  saveToken(tokenModel: TokenModel)
  {
    localStorage.setItem('auth', JSON.stringify(tokenModel));
    this.jwt = tokenModel;
  }
  isConnected()
  {
    return this.jwt != null;
  }
  isAdmin()
  {
    return this.jwt.isAdmin;
  }
  getToken()
  {
    return this.jwt.token;
  }
  removeToken()
  {
    this.jwt = null;
    localStorage.removeItem("auth");
  }
}
