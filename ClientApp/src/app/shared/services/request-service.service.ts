import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { RequestModel } from '../models/requestModel';

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
  getRequests(): Observable<RequestModel[]> {
    return this.http.get<RequestModel[]>(this.baseUrl + '/requests/list');
  }
  getRequestsByUser(userId: string): Observable<RequestModel[]>
  {
    return this.http.get<RequestModel[]>(this.baseUrl + `/requests/list/${userId}`);
  }
  validateRequests(requestList: Array<number>): Observable<RequestModel[]> {
    return this.http.post<RequestModel[]>(this.baseUrl + '/requests/validate', requestList);
  }
  deleteRequests(requestList: Array<number>): Observable<RequestModel[]> {
    return this.http.post<RequestModel[]>(this.baseUrl + '/requests/delete', requestList);
  }
}
