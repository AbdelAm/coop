import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { RequestModel } from '../models/request-model';

@Injectable({
  providedIn: 'root'
})
export class RequestServiceService {

  readonly baseUrl = environment.apiUrl;
  request: RequestModel;
  constructor(private http: HttpClient) {
    this.request = new RequestModel();
  }
  setRequest(): Observable<RequestModel> {
    return this.http.post<RequestModel>(this.baseUrl + 'request/add', this.request);
  }
  getRequests(page: number = 1): Observable<RequestModel[]> {
    return this.http.get<RequestModel[]>(this.baseUrl + `request/list/${page}`);
  }
  getRequestsByUser(userId: string, page: number = 1): Observable<RequestModel[]> {
    return this.http.get<RequestModel[]>(this.baseUrl + `request/list/${userId}/${page}`);
  }
  validateRequests(requestList: Array<number>): Observable<RequestModel[]> {
    return this.http.post<RequestModel[]>(this.baseUrl + 'request/validate', requestList);
  }

  rejectRequests(requestList: Array<number>, page: number = 1): Observable<RequestModel[]> {
    return this.http.post<RequestModel[]>(this.baseUrl + `request/reject`, requestList);
  }
  deleteRequests(requestList: Array<number>): Observable<RequestModel[]> {
    return this.http.post<RequestModel[]>(this.baseUrl + 'request/delete', requestList);
  }
}
