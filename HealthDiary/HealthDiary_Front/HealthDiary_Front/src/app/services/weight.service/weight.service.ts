import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
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
          return throwError(() => new Error('Failed to fetch weather info'));
        })
      );
  }
}
