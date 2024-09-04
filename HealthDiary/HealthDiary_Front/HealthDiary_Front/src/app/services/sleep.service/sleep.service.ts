import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, of, throwError } from 'rxjs';
import { Result } from 'src/app/models/operation-result';
import { SleepInfoDto } from 'src/app/models/sleep-info-dto';

@Injectable({
  providedIn: 'root'
})
export class SleepService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = 'https://localhost:7241/api/sleep/';

  public getLastSleepInformationByUserId(paramValue: number): Observable<SleepInfoDto> {
    const params = new HttpParams().set('Id', paramValue);
    
    return this.http.get<SleepInfoDto>(`${this.baseUrl}getSleepInfo`, { params: params }).pipe(
      catchError(error => {
        console.error("Sleep API error:", error);
        return throwError(() => new Error('Failed to fetch sleep info'));
      })
    );
  }
}
