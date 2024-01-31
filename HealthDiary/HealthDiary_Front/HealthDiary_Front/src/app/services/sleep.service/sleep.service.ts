import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Result } from 'src/app/models/operation-result';
import { SleepInfoDto } from 'src/app/models/sleep-info-dto';

@Injectable({
  providedIn: 'root'
})
export class SleepService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = 'https://localhost:7241/api/sleep/';

  public getLastSleepInformationByUserId(paramValue: number): Observable<Result<SleepInfoDto>> {
    const params = new HttpParams().set('Id', paramValue);
    return this.http.get<Result<SleepInfoDto>>(`${this.baseUrl}GetLastSleepInformationByUserId`, { params: params });
  }
}
