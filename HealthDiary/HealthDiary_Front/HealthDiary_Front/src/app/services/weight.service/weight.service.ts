import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map, of } from 'rxjs';
import { Result } from 'src/app/models/operation-result';
import { WeightDto } from 'src/app/models/weight-dto';

@Injectable({
  providedIn: 'root'
})
export class WeightService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = 'https://localhost:7241/api/weight/';

  public getUserWeightsByMonth(paramValue: number): Observable<Array<WeightDto>> {
    const params = new HttpParams().set('Id', paramValue.toString());

    return this.http.get<Array<WeightDto>>(`${this.baseUrl}getWeightsByMonth`, { params })
      .pipe(
        catchError(error => {
          console.error('Error fetching weights:', error);
          return of([]); 
        })
      );
  }

  public getUserYearlyWeightById(paramValue: number): Observable<Result<Array<WeightDto>>> {
    const params = new HttpParams().set('Id', paramValue);
    return this.http.get<Result<Array<WeightDto>>>(`${this.baseUrl}getYearlyWeightById`, { params: params });
  }
}
