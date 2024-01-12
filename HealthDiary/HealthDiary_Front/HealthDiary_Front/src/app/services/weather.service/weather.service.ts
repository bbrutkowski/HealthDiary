import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { OperationResult } from 'src/app/models/operation-result';

@Injectable({
  providedIn: 'root'
})
export class WeatherService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = 'https://localhost:7241/api/WeatherInfo/'

  public getWeather() : Observable<OperationResult<Boolean>>{
    return this.http.get<OperationResult<Boolean>>(`${this.baseUrl}GetWeather`);
  }
}
