import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, take, throwError } from 'rxjs';
import { WeightDto } from 'src/app/models/weight-dto';
import { WeightGoalDto } from 'src/app/models/weight-goal-dto';

@Injectable({
  providedIn: 'root'
})
export class WeightService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = 'https://localhost:7241/api/weight/';
  private weightErrorMessage = 'Weight API error:'

  public getUserWeightsByMonth(paramValue: number): Observable<Array<WeightDto>> {
    const params = new HttpParams().set('Id', paramValue.toString());

    return this.http.get<Array<WeightDto>>(`${this.baseUrl}getWeightsByMonth`, { params })
      .pipe(
        catchError(error => {
          console.error('Error fetching weights:', error);
          return throwError(() => new Error('Failed to fetch weights info'));
        })
      );
  }

  public getWeightGoal(userId: number): Observable<WeightGoalDto> {
    const params = new HttpParams().set('Id', userId.toString());

    return this.http.get<WeightGoalDto>(`${this.baseUrl}getWeightGoal`, { params })
      .pipe(
        catchError(error => {
          console.error('Error fetching weight goal:', error);
          return throwError(() => new Error('Failed to fetch weight goal'));
        })
      );
  }

  public saveWeightGoal(weightGoalData: any) : Observable<boolean>{
    return this.http.post<boolean>(`${this.baseUrl}saveWeightGoal`, weightGoalData).pipe(
      take(1),
      catchError(err => {
        console.error(this.weightErrorMessage, err);
        return throwError(() => new Error('Failed to save weight goal'))
      })
    );
  }
}
