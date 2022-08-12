import {HttpClient} from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {environment} from 'src/environments/environment';
import {RequestModel} from '../models/request-model';

@Injectable({
  providedIn: 'root',
})
export class RequestServiceService {
  readonly baseUrl = environment.apiUrl;

  request: RequestModel;

  constructor(private http: HttpClient) {
    this.request = new RequestModel();
  }

  setRequest(request: RequestModel): Observable<RequestModel> {
    return this.http.post<RequestModel>(this.baseUrl + 'request/add', request);
  }

  getRequests(pageNumber: number, pageSize: number): Observable<GetRequestResponse> {
    return this.http.get<GetRequestResponse>(this.baseUrl + 'request?pageNumber=' + pageNumber + '&pageSize=' + pageSize);
  }

  getRequestsByUser(userId: string, pageNumber: number, pageSize: number): Observable<GetRequestResponse> {
    return this.http.get<GetRequestResponse>(this.baseUrl + 'request/user/' + userId + '?pageNumber=' + pageNumber + '&pageSize=' + pageSize);
  }

  searchRequest(value: string): Observable<RequestModel[]> {
    return this.http.get<RequestModel[]>(this.baseUrl + `request/${value}`);
  }

  validateRequests(requestList: Array<number>): Observable<RequestModel[]> {
    return this.http.post<RequestModel[]>(
      this.baseUrl + 'request/validate',
      requestList
    );
  }

  rejectRequests(requestList: Array<number>): Observable<RequestModel[]> {
    return this.http.post<RequestModel[]>(
      this.baseUrl + `request/reject`,
      requestList
    );
  }

  deleteRequests(requestList: Array<number>): Observable<RequestModel[]> {
    return this.http.post<RequestModel[]>(
      this.baseUrl + 'request/delete',
      requestList
    );
  }
}

interface GetRequestResponse {
  response: {
    request: RequestModel[];
  },
  pagination: {
    pageNumber: number,
    pageSize: number,
    totalRecords: number
  };
}

