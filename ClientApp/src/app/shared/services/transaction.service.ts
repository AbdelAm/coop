import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {TransactionModel} from '../models/transaction-model';

@Injectable({
  providedIn: 'root',
})
export class TransactionService {
  readonly baseUrl = environment.apiUrl + 'transactions';
  transaction: TransactionModel;

  constructor(private httpClient: HttpClient) {
    this.transaction = new TransactionModel();
  }

  postTransaction(transaction: TransactionModel) {
    return this.httpClient.post(this.baseUrl, transaction);
  }

  getTransactionsByUser(
    userBankAccountId: number,
    pageNumber = 1,
    pageSize: number
  ): Observable<GetTransactionsByUserResponse> {
    return this.httpClient.get<GetTransactionsByUserResponse>(
      this.baseUrl +
      '/user/' +
      userBankAccountId +
      '?pageNumber=' +
      pageNumber +
      '&pageSize=' +
      pageSize
    );
  }

  getTransactions(
    pageNumber: number,
    pageSize: number
  ): Observable<GetTransactionsResponse> {
    return this.httpClient.get<GetTransactionsResponse>(
      this.baseUrl + '?pageNumber=' + pageNumber + '&pageSize=' + pageSize
    );
  }

  validateTransaction(transactionId: number) {
    return this.httpClient.get(this.baseUrl + '/validate/' + transactionId);
  }

  rejectTransaction(transactionId: number) {
    return this.httpClient.get(this.baseUrl + '/reject/' + transactionId);
  }

  removeTransaction(transactionId: number) {
    return this.httpClient.delete(this.baseUrl + '/ ' + transactionId);
  }

  validateAllTransaction(transactionsIds: number[]) {
    return this.httpClient.post(
      this.baseUrl + '/validate-all',
      transactionsIds
    );
  }

  rejectAllTransaction(transactionsIds: number[]) {
    return this.httpClient.post(this.baseUrl + '/reject-all', transactionsIds);
  }

  removeAllTransaction(transactionsIds: number[]) {
    return this.httpClient.post(this.baseUrl + '/remove-all', transactionsIds);
  }

  handleTransactionSearch(
    keyword: string,
    pageNumber: number,
    pageSize: number
  ): Observable<GetTransactionsResponse> {
    return this.httpClient.get<GetTransactionsResponse>(
      this.baseUrl +
      /search/ +
      keyword +
      '?pageNumber=' +
      pageNumber +
      '&pageSize=' +
      pageSize
    );
  }
}

interface GetTransactionsResponse {
  response: {
    transactions: TransactionModel[];
  };
  pagination: {
    pageNumber: number;
    pageSize: number;
    totalRecords: number;
  };
}

interface GetTransactionsByUserResponse {
  response: {
    TransactionsSent: TransactionModel[];
    TransactionsReceived: TransactionModel[];
  };
  pagination: {
    pageNumber: number;
    pageSize: number;
    totalRecords: number;
  };
}
