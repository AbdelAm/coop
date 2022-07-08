import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserModel } from '../models/userModel';

@Injectable({
  providedIn: 'root'
})
export class UserServiceService {

  readonly baseUrl = environment.apiUrl;
  user: UserModel;

  constructor(private http: HttpClient) {
    this.user = new UserModel();
  }

  setUser(): Observable<UserModel> {
    return this.http.post<UserModel>(this.baseUrl + '/admin/users/add', this.user);
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
