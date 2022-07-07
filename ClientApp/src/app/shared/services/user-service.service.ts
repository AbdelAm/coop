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
    return this.http.post<UserModel>(this.baseUrl + '/users/add', this.user);
  }
  setUsers(): Observable<UserModel[]> {
    return this.http.get<UserModel[]>(this.baseUrl + '/users/list');
  }
  validateUsers(userList: Array<number>): Observable<UserModel[]> {
    return this.http.post<UserModel[]>(this.baseUrl + '/users/validate', userList);
  }
  deleteUsers(userList: Array<number>): Observable<UserModel[]> {
    return this.http.post<UserModel[]>(this.baseUrl + '/users/delete', userList);
  }
}
