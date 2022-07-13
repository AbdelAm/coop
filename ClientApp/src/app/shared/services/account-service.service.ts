import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { BankAccountModel } from '../models/bankAccount-model';

@Injectable({
  providedIn: 'root'
})
export class AccountServiceService {

  readonly baseUrl = environment.apiUrl;
  bankAccount: BankAccountModel
  constructor(private http: HttpClient) {
    this.bankAccount = new BankAccountModel();
  }
  setAccount(): Observable<BankAccountModel> {
    return this.http.post<BankAccountModel>(this.baseUrl + '/accounts/add', this.bankAccount);
  }
  getAccounts(): Observable<BankAccountModel[]> {
    return this.http.get<BankAccountModel[]>(this.baseUrl + '/accounts/list');
  }
  validateAccounts(accountList: Array<number>): Observable<BankAccountModel[]> {
    return this.http.post<BankAccountModel[]>(this.baseUrl + '/accounts/validate', accountList);
  }
  deleteAccounts(accountList: Array<number>): Observable<BankAccountModel[]> {
    return this.http.post<BankAccountModel[]>(this.baseUrl + '/accounts/delete', accountList);
  }
}
