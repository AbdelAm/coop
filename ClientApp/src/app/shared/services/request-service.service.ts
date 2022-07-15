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
    return this.http.post<RequestModel>(this.baseUrl + '/requests/add', this.request);
  }
  getRequests(page: number = 1): Observable<RequestModel[]> {
    return this.http.get<RequestModel[]>(this.baseUrl + `/admin/requests/list/${page}`);
  }
  getRequestsByUser(userId: string, page: number = 1): Observable<RequestModel[]> {
    return this.http.get<RequestModel[]>(this.baseUrl + `/requests/list/${userId}/${page}`);
  }
  validateRequests(requestList: Array<number>): Observable<RequestModel[]> {
    return this.http.post<RequestModel[]>(this.baseUrl + '/admin/requests/validate', requestList);
  }
  deleteRequests(requestList: Array<number>): Observable<RequestModel[]> {
    return this.http.post<RequestModel[]>(this.baseUrl + '/admin/requests/delete', requestList);
  }
}
