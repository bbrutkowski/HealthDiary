import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Result } from 'src/app/models/operation-result';
import { WeatherDto } from 'src/app/models/weather-dto';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = 'https://localhost:7241/api/WeatherInfo/'

  public getWeather() : Observable<Result<WeatherDto>>{
    return this.http.get<Result<WeatherDto>>(`${this.baseUrl}GetWeather`);
  }
}
