import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, of, throwError } from 'rxjs';
import { Result } from 'src/app/models/operation-result';
import { TotalActivityDto } from 'src/app/models/total-activity';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = 'https://localhost:7241/api/activity/';

  public getMonthlyActivityByUserId(paramValue: number): Observable<TotalActivityDto> {
    const params = new HttpParams().set('Id', paramValue);

    return this.http.get<TotalActivityDto>(`${this.baseUrl}getActivity`, { params: params })
      .pipe(
        catchError(error => {
          console.error('Error fetching activities:', error);
          return throwError(() => new Error('Failed to fetch activities'));
        })
    )
  }
}
