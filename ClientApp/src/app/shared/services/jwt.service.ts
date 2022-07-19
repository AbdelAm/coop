import { HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TokenModel } from '../models/token-model';

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  jwt!: TokenModel;

  constructor() {
    let res = localStorage.getItem('auth');
    if(res) this.jwt = JSON.parse(res); 
  }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if(this.jwt){
            req = req.clone({
            headers: req.headers.set('Authorisation', `${this.jwt.token}`)});
    }
    return next.handle(req);
  }
}
