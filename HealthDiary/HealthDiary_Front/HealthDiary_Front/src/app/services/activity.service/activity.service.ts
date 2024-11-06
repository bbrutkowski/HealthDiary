import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, of, take, throwError } from 'rxjs';
import { ActivityCatalog } from 'src/app/models/activity-catalog';
import { TotalActivityDto } from 'src/app/models/total-activity';
import { WeeklyActivity } from 'src/app/models/weekly-activity';

@Injectable({
  providedIn: 'root'
})
export class ActivityService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = 'https://localhost:7241/api/activity/';
  private activityErrorMessage = 'Activity API error:'

  public getMonthlyActivityByUserId(paramValue: number): Observable<TotalActivityDto> {
    const params = new HttpParams().set('Id', paramValue);

    return this.http.get<TotalActivityDto>(`${this.baseUrl}get-activity`, { params: params })
      .pipe(
        catchError(error => {
          console.error(this.activityErrorMessage, error);
          return throwError(() => new Error('Failed to fetch monthly activities'));
        })
    )
  }

  public getActivities(paramValue: number): Observable<ActivityCatalog> {
    const params = new HttpParams().set('Id', paramValue);

    return this.http.get<ActivityCatalog>(`${this.baseUrl}get-activities`, {params: params})
      .pipe(
        catchError(error => {
          console.error(this.activityErrorMessage, error);
          return throwError(() => new Error('Failed to fetch activities'));
        })
    )
  }

  public saveActivity(activityData: any) : Observable<boolean> {
    return this.http.post<boolean>(`${this.baseUrl}save-activity`, activityData).pipe(
      catchError(err => {
        console.error(this.activityErrorMessage, err);
        return throwError(() => new Error('Failed to save activity'))
      })
    );
  }

  public getWeeklyActivities(paramValue: number): Observable<Array<WeeklyActivity>> {
    const params = new HttpParams().set('Id', paramValue);

    return this.http.get<Array<WeeklyActivity>>(`${this.baseUrl}get-weekly-activity`, {params: params})
      .pipe(
        catchError(error => {
          console.error(this.activityErrorMessage, error);
          return throwError(() => new Error('Failed to fetch activities'));
        })
    )
  }
}
