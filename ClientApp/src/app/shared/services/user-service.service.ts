import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {EmailUpdateModel} from '../models/email-update-model';
import {ItemsModel} from '../models/items-model';
import {PasswordUpdateModel} from '../models/password-update-model';
import { UserBankItemModel } from '../models/user-bank-item-model';
import {UserInfoModel} from '../models/user-info-model';
import {UserItemModel} from '../models/user-item-model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  readonly baseUrl = environment.apiUrl;
  progressNumber : number;

  constructor(private http: HttpClient) {
    this.progressNumber = 0;
  }

  setProgressNumber(count: number)
  {
    this.progressNumber = count;
  }
  getProgressNumber()
  {
    return this.progressNumber;
  }
  getUsers(page: number = 0): Observable<ItemsModel<UserItemModel>> {
    return this.http.get<ItemsModel<UserItemModel>>(
      this.baseUrl + `user/list/${page}`
    );
  }

  getProgressUsers(): Observable<number> {
    return this.http.get<number>(this.baseUrl + `user/progress/list`);
  }

  getUser(cif: string): Observable<UserBankItemModel> {
    return this.http.get<UserBankItemModel>(this.baseUrl + `user/${cif}`);
  }

  searchUser(value: string): Observable<UserItemModel[]> {
    return this.http.get<UserItemModel[]>(
      this.baseUrl + `user/search/${value}`
    );
  }

  validateUsers(userList: Array<string>): Observable<Response> {
    return this.http.post<Response>(this.baseUrl + `user/validate`, userList);
  }

  rejectUsers(userList: Array<string>): Observable<Response> {
    return this.http.post<Response>(this.baseUrl + `user/reject`, userList);
  }

  deleteUsers(userList: Array<string>): Observable<Response> {
    return this.http.post<Response>(this.baseUrl + `user/delete`, userList);
  }

  updateUserInfo(userInfo: UserInfoModel): Observable<Response> {
    return this.http.put<Response>(this.baseUrl + `user/update/info`, userInfo);
  }

  updateEmail(userEmail: EmailUpdateModel): Observable<Response> {
    return this.http.put<Response>(
      this.baseUrl + `user/update/email`,
      userEmail
    );
  }

  updatePassword(userPassword: PasswordUpdateModel): Observable<Response> {
    return this.http.put<Response>(
      this.baseUrl + `user/update/password`,
      userPassword
    );
  }
}
