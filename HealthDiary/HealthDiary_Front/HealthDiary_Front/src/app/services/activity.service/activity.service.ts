import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Result } from 'src/app/models/operation-result';
import { TotalActivityDto } from 'src/app/models/total-activity';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = 'https://localhost:7241/api/activity/';

  public getMonthlyActivityByUserId(paramValue: number): Observable<Result<TotalActivityDto>> {
    const params = new HttpParams().set('Id', paramValue);
    return this.http.get<Result<TotalActivityDto>>(`${this.baseUrl}GetMonthlyActivityByUserId`, { params: params });
  }
}
