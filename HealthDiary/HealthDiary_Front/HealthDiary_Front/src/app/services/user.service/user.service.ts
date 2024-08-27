import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Result } from 'src/app/models/operation-result';
import { UserDto } from 'src/app/models/user-dto';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl: string = 'https://localhost:7241/api/user/'

  constructor(private http: HttpClient) { }

  public register(registerData: any) : Observable<Result<Boolean>>{
    return this.http.post<Result<Boolean>>(`${this.baseUrl}register`, registerData);
  }

  public getUserById(paramValue: number): Observable<Result<UserDto>>{
    const params = new HttpParams().set('Id', paramValue);
    return this.http.get<Result<UserDto>>(`${this.baseUrl}getUserInfo`, { params: params });
  }

  public updateUser(userData: UserDto) : Observable<Result<Boolean>> {
    return this.http.post<Result<Boolean>>(`${this.baseUrl}update`, userData);
  }
}
