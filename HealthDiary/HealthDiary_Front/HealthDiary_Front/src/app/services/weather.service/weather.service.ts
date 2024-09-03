import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = 'https://localhost:7241/api/weather/'

  public getWeather(): Observable<string> {
    return this.http.get(`${this.baseUrl}getWeather`, { responseType: 'text' }).pipe(
      catchError(error => {
        console.error('Weather API error:', error);
        return of(`Error: ${error.message}`);
      })
    );
  }
}
