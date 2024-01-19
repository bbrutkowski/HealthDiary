import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { OperationResult } from 'src/app/models/operation-result';
import { UserDto } from 'src/app/models/user-dto';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl: string = 'https://localhost:7241/api/User/'

  constructor(private http: HttpClient) { }

  public register(registerData: any) : Observable<OperationResult<Boolean>>{
    return this.http.post<OperationResult<Boolean>>(`${this.baseUrl}Register`, registerData);
  }

  public getUserById(paramValue: number): Observable<OperationResult<UserDto>>{
    const params = new HttpParams().set('Id', paramValue);
    return this.http.get<OperationResult<UserDto>>(`${this.baseUrl}GetUserById`, { params: params });
  }

  public updateUser(userData: UserDto) : Observable<OperationResult<Boolean>> {
    return this.http.post<OperationResult<Boolean>>(`${this.baseUrl}Update`, userData);
  }
}
