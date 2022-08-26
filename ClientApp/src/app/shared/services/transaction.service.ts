import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable, Subject} from 'rxjs';
import {environment} from 'src/environments/environment';
import {TransactionModel} from '../models/transaction-model';
import {TransactionPostModel} from '../models/Transaction-post-model';
import {saveAs} from 'file-saver';

@Injectable({
  providedIn: 'root',
})
export class TransactionService {
  readonly baseUrl = environment.apiUrl + 'transactions';
  transaction: TransactionModel;

  refreshTransactions;

  constructor(private httpClient: HttpClient) {
    this.transaction = new TransactionModel();
    this.refreshTransactions = new Subject();
  }

  postTransaction(transaction: TransactionPostModel) {
    return this.httpClient.post(this.baseUrl, transaction);
  }

  refresh() {
    this.refreshTransactions.next();
  }

  countInProgressTransactions(): Observable<number> {
    return this.httpClient.get<number>(this.baseUrl + '/count');
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

  getAllTransactionsByStatus(status: string, pageNumber: number,
                             pageSize: number): Observable<GetTransactionsResponse> {
    return this.httpClient.get<GetTransactionsResponse>(
      this.baseUrl + '/filter/' + status + '?pageNumber=' + pageNumber + '&pageSize=' + pageSize
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

  importAsCsv(bankAccountId: number) {

    this.httpClient.get(this.baseUrl + '/csv/' + bankAccountId, {responseType: 'arraybuffer'}).subscribe(res =>
      this.downLoadFile(res, 'text/csv')
    );
  }

  importAsExcel(bankAccountId: number) {
    return this.httpClient.get(this.baseUrl + '/excel/' + bankAccountId, {responseType: 'arraybuffer'}).subscribe(res =>
      this.downLoadFile(res, 'application/vnd.ms-excel ')
    );

  }

  importAsPdf(bankAccountId: number) {
    return this.httpClient.get(this.baseUrl + '/pdf/' + bankAccountId, {responseType: 'arraybuffer'}).subscribe(res =>
      this.downLoadFile(res, 'application/pdf')
    );

  }

  importAllTransactionsAsCsv() {

    this.httpClient.get(this.baseUrl + '/admin/csv', {responseType: 'arraybuffer'}).subscribe(res =>
      this.downLoadFile(res, 'text/csv')
    );
  }

  importAllTransactionsAsExcel() {
    return this.httpClient.get(this.baseUrl + '/admin/excel', {responseType: 'arraybuffer'}).subscribe(res =>
      this.downLoadFile(res, 'application/vnd.ms-excel ')
    );

  }

  importAllTransactionsAsPdf() {
    return this.httpClient.get(this.baseUrl + '/admin/pdf', {responseType: 'arraybuffer'}).subscribe(res =>
      this.downLoadFile(res, 'application/pdf')
    );

  }

  /**
   * Method is used to download file.
   * @param data - Array Buffer data
   * @param type - type of the document.
   */
  downLoadFile(data: any, type: string) {
    const blob = new Blob([data], {type: type});
    saveAs(blob, 'Transacciones');
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
