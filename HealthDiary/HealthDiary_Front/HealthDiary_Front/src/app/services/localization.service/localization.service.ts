import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, take, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LocalizationService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = 'https://localhost:7241/api/localization/';

  public getCityName(latitude: number, longitude: number): Observable<string> {
    return this.http.post<string>(`${this.baseUrl}getCity`, { latitude: latitude, longitude: longitude }).pipe(
      take(1),
      catchError(error => {
        console.error('Localization API error:', error);
        return throwError(() => new Error('Failed to fetch city name'));
      })
    );
  }
}
