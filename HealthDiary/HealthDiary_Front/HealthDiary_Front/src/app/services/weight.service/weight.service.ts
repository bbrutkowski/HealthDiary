import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, take, throwError } from 'rxjs';
import { BmiDto } from 'src/app/models/bmi-dto';
import { BmiDataDto } from 'src/app/models/bmi-request';
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

    return this.http.get<Array<WeightDto>>(`${this.baseUrl}get-weights-by-month`, { params })
      .pipe(
        catchError(error => {
          console.error('Error fetching weights:', error);
          return throwError(() => new Error('Failed to fetch weights info'));
        })
      );
  }

  public getWeightGoal(userId: number): Observable<WeightGoalDto> {
    const params = new HttpParams().set('Id', userId.toString());

    return this.http.get<WeightGoalDto>(`${this.baseUrl}get-weight-goal`, { params })
      .pipe(
        catchError(error => {
          console.error(this.weightErrorMessage, error);
          return throwError(() => new Error('Failed to fetch weight goal'));
        })
      );
  }

  public saveWeightGoal(weightGoalData: any) : Observable<boolean>{
    return this.http.post<boolean>(`${this.baseUrl}save-weight-goal`, weightGoalData).pipe(
      take(1),
      catchError(err => {
        console.error(this.weightErrorMessage, err);
        return throwError(() => new Error('Failed to save weight goal'))
      })
    );
  }

  public getYearlyWeight(paramValue: number): Observable<Array<WeightDto>> {
    const params = new HttpParams().set('Id', paramValue.toString());

    return this.http.get<Array<WeightDto>>(`${this.baseUrl}get-yearly-weight`, { params })
      .pipe(
        catchError(error => {
          console.error(this.weightErrorMessage, error);
          return throwError(() => new Error('Failed to fetch weights info'));
        })
      );
  }

  public getBMI(paramValue: number): Observable<BmiDto> {
    const params = new HttpParams().set('id', paramValue.toString());

    return this.http.get<BmiDto>(`${this.baseUrl}get-bmi`, { params })
      .pipe(
        catchError(error => {
          console.error(this.weightErrorMessage, error);
          return throwError(() => new Error('Failed to fetch BMI'));
        })
      );
  }

  public saveBMI(bmiData: BmiDataDto) : Observable<boolean>{
    return this.http.post<boolean>(`${this.baseUrl}save-bmi`, bmiData).pipe(
      take(1),
      catchError(err => {
        console.error(this.weightErrorMessage, err);
        return throwError(() => new Error('Failed to save BMI'))
      })
    );
  }

  public getWeightGoalProgress(userId: number): Observable<number> {
    const params = new HttpParams().set('Id', userId.toString());

    return this.http.get<number>(`${this.baseUrl}get-weight-goal-progress`, { params })
      .pipe(
        catchError(error => {
          console.error(this.weightErrorMessage, error);
          return throwError(() => new Error('Failed to fetch weight goal'));
        })
      );
  }

  public addWeight(weightData: any) : Observable<boolean>{
    return this.http.post<boolean>(`${this.baseUrl}add-weight`, weightData).pipe(
      take(1),
      catchError(err => {
        console.error(this.weightErrorMessage, err);
        return throwError(() => new Error('Failed to save weight'))
      })
    );
  }
}
