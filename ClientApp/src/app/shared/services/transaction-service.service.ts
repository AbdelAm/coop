import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { TransactionModel } from '../models/transaction-model';

@Injectable({
  providedIn: 'root'
})
export class TransactionServiceService {

  readonly baseUrl = environment.apiUrl;
  transaction: TransactionModel;

  constructor(private http: HttpClient) {
    this.transaction = new TransactionModel();
  }

  setTransaction(): Observable<TransactionModel> {
    return this.http.post<TransactionModel>(this.baseUrl + '/transactions/add', this.transaction);
  }
  getTransactions(page: number = 1): Observable<TransactionModel[]> {
    return this.http.get<TransactionModel[]>(this.baseUrl + `/admin/transactions/list/${page}`);
  }
  getTransactionsByUser(userId: string, page: number = 1): Observable<TransactionModel[]>
  {
    return this.http.get<TransactionModel[]>(this.baseUrl + `/transactions/list/${userId}/${page}`);
  }
  validateTransactions(userList: Array<number>): Observable<TransactionModel[]> {
    return this.http.post<TransactionModel[]>(this.baseUrl + '/admin/transactions/validate', userList);
  }
  deleteTransactions(userList: Array<number>): Observable<TransactionModel[]> {
    return this.http.post<TransactionModel[]>(this.baseUrl + '/admin/transactions/delete', userList);
  }
}
