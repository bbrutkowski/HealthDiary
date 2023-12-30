import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';

export function createParams(data: Record<string, any>): HttpParams {
  let params = new HttpParams();
  Object.keys(data).forEach(function (key) {
    params = params.append(key, data[key])
  });
  return params;
}

export const httpOptions = {
  headers: new HttpHeaders({
      'Content-Type': 'application/json',
  }),
};

@Injectable()
export abstract class RequestHelperBaseService {
  protected actionUrl: string = '';
  public basePath: string = '';
  public apiPath: string = '';

  protected abstract getApiRoute(): string;

  constructor(protected http: HttpClient) { }

  public add<T>(item: any): Observable<T> {
    const toAdd = JSON.stringify({item: item});
    return this.http.post<T>(this.actionUrl, toAdd);   
  }

  public get<T>(id: number): Observable<T>{
    return this.http.get<T>(this.actionUrl + '/' + id);
  }

  public update<T>(id: number, toUpdate: any): Observable<T> {
    return this.http.put<T>(this.actionUrl + id, JSON.stringify(toUpdate));
  }

  public delete<T>(id: any): Observable<T> {
    return this.http.delete<T>(this.actionUrl + '/' + id);
  }

  protected createGetRequest<T>(route: string): Observable<T> {
    const url = this.actionUrl + '/' + route;
    return this.http.get<T>(url);
  }

  protected createGetRequestWithParams<T>(route: string, data: Object): Observable<T> {
    const params = createParams(data);
    const url = this.actionUrl + '/' + route;
    return this.http.get<T>(url, {params: params});
  }

  protected createPostRequest<T>(route: string, data: Object, params?: HttpParams): Observable<T> {
    const url = this.actionUrl + '/' + route;
    return this.http.post<T>(url, JSON.stringify(data), httpOptions);    
  }
}
