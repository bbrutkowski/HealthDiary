import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, take, throwError } from 'rxjs';
import { WeatherResponseDto } from 'src/app/models/weather-response-dto';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = 'https://localhost:7241/api/weather/'

  public getWeather(latitude: number, longitude: number): Observable<WeatherResponseDto> {
    return this.http.post<WeatherResponseDto>(`${this.baseUrl}getWeather`, { latitude: latitude, longitude: longitude }).pipe(
      take(1),
      catchError(error => {
        console.error('Weather API error:', error);
        return throwError(() => new Error('No weather conditions'));
      })
    ); 
  }
}
