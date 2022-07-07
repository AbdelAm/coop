import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { TransactionModel } from '../models/transactionModel';

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
  getTransactions(): Observable<TransactionModel[]> {
    return this.http.get<TransactionModel[]>(this.baseUrl + '/transactions/list');
  }
  getTransactionsByUser(userId: string): Observable<TransactionModel[]>
  {
    return this.http.get<TransactionModel[]>(this.baseUrl + `/transactions/list/${userId}`);
  }
  validateTransactions(userList: Array<number>): Observable<TransactionModel[]> {
    return this.http.post<TransactionModel[]>(this.baseUrl + '/transactions/validate', userList);
  }
  deleteTransactions(userList: Array<number>): Observable<TransactionModel[]> {
    return this.http.post<TransactionModel[]>(this.baseUrl + '/transactions/delete', userList);
  }
}
