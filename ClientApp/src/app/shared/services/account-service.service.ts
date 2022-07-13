import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AccountModel } from '../models/account-model';

@Injectable({
  providedIn: 'root'
})
export class AccountServiceService {

  readonly baseUrl = environment.apiUrl;
  account: AccountModel
  constructor(private http: HttpClient) {
    this.account = new AccountModel();
  }
  setAccount(): Observable<AccountModel> {
    return this.http.post<AccountModel>(this.baseUrl + '/accounts/add', this.account);
  }
  getAccounts(): Observable<AccountModel[]> {
    return this.http.get<AccountModel[]>(this.baseUrl + '/accounts/list');
  }
  validateAccounts(accountList: Array<number>): Observable<AccountModel[]> {
    return this.http.post<AccountModel[]>(this.baseUrl + '/accounts/validate', accountList);
  }
  deleteAccounts(accountList: Array<number>): Observable<AccountModel[]> {
    return this.http.post<AccountModel[]>(this.baseUrl + '/accounts/delete', accountList);
  }
}
