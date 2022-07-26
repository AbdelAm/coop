import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {TransactionModel} from '../models/transaction-model';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  readonly baseUrl = environment.apiUrl + 'transactions';
  transaction: TransactionModel;

  constructor(private httpClient: HttpClient) {
    this.transaction = new TransactionModel();
  }

  getTransactionsByUser(userBankAccountId): Observable<TransactionModel[]> {
    return this.httpClient.get<TransactionModel[]>(this.baseUrl + '/user/' + userBankAccountId);
  }

  getTransactions(): Observable<TransactionModel[]> {
    return this.httpClient.get<TransactionModel[]>(this.baseUrl + 'transactions');
  }

  validateTransaction(transactionId: number) {
    return this.httpClient.get(this.baseUrl + '/validate/' + transactionId);
  }

  rejectTransaction(transactionId: number) {
    return this.httpClient.get(this.baseUrl + '/reject/' + transactionId);
  }

  removeTransaction(transactionId: number) {
    return this.httpClient.delete(this.baseUrl + '/' + transactionId);
  }

  updateTransaction() {
    // TO DO
  }

  validateAllTransaction(transactionsIds: number[]) {
    return this.httpClient.post(this.baseUrl + '/validate-all', transactionsIds);
  }


  rejectAllTransaction(transactionsIds: number[]) {
    return this.httpClient.post(this.baseUrl + '/reject-all', transactionsIds);
  }

  removeAllTransaction(transactionsIds: number[]) {
    return this.httpClient.post(this.baseUrl + '/remove-all', transactionsIds);
  }

}
