import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, take, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = 'https://localhost:7241/api/weather/'

  public getWeather(latitude: number, longitude: number): Observable<string> {
    return this.http.post<string>(`${this.baseUrl}getWeather`, { latitude: latitude, longitude: longitude }, {responseType: 'text' as 'json'}).pipe(
      take(1),
      catchError(error => {
        console.error('Weather API error:', error);
        return throwError(() => new Error('Failed to fetch weather conditions'));
      })
    ); 
  }
}
