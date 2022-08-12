import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {BankAccountModel} from '../models/bankAccount-model';

@Injectable({
  providedIn: 'root',
})
export class BankAccountService {
  readonly baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) {
  }

  setBankAccount(bankAccount: BankAccountModel): Observable<Response> {
    return this.http.post<Response>(
      this.baseUrl + 'bankaccount/add',
      bankAccount
    );
  }

  getAccounts(): Observable<BankAccountModel[]> {
    return this.http.get<BankAccountModel[]>(this.baseUrl + 'bankaccount/list');
  }

  getBankAccount(userId: string): Observable<BankAccountModel> {
    return this.http.get<BankAccountModel>(
      this.baseUrl + `bankaccount/${userId}`
    );
  }

  validateAccounts(accountList: Array<number>): Observable<BankAccountModel[]> {
    return this.http.post<BankAccountModel[]>(
      this.baseUrl + 'bankaccount/validate',
      accountList
    );
  }

  deleteAccounts(accountList: Array<number>): Observable<BankAccountModel[]> {
    return this.http.post<BankAccountModel[]>(
      this.baseUrl + 'bankaccount/delete',
      accountList
    );
  }
}
