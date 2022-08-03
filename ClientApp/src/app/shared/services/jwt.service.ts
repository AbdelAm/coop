import {Injectable} from '@angular/core';
import {TokenModel} from '../models/token-model';

@Injectable({
  providedIn: 'root'
})
export class JwtService {

  jwt!: TokenModel;
  switchBtn: boolean;

  constructor() {
    let res = localStorage.getItem('auth');
    if (res) {
      this.jwt = JSON.parse(res);
    }
    this.switchBtn = true;
  }

  saveToken(tokenModel: TokenModel) {
    this.jwt = tokenModel;
    localStorage.setItem('auth', JSON.stringify(tokenModel));
    return true;
  }

  isConnected() {
    return this.jwt != null;
  }

  isAdmin() {
    return this.jwt.isAdmin;
  }

  getToken() {
    return this.jwt.token;
  }

  removeToken() {
    this.jwt = null;
    localStorage.removeItem('auth');
  }

  getConnectedUserId() {
    return this.jwt.cif;
  }
}
