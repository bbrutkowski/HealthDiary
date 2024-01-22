import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Result } from 'src/app/models/operation-result';
import { WeightDto } from 'src/app/models/weight-dto';

@Injectable({
  providedIn: 'root'
})
export class WeightService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = 'https://localhost:7241/api/Weight/';

  public getWeightsByUserId(paramValue: number): Observable<Result<Array<WeightDto>>> {
    const params = new HttpParams().set('Id', paramValue);
    return this.http.get<Result<Array<WeightDto>>>(`${this.baseUrl}GetWeightsByUserId`, { params: params })
  }

  public getUserWeightsByMonth(paramValue: number): Observable<Result<Array<WeightDto>>> {
    const params = new HttpParams().set('Id', paramValue);
    return this.http.get<Result<Array<WeightDto>>>(`${this.baseUrl}GetWeightsByMonth`, { params: params });
  }
}
