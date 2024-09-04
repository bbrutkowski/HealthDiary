import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, of, take, throwError } from 'rxjs';
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

  public getUserById(paramValue: number): Observable<UserDto>{
    const params = new HttpParams().set('Id', paramValue);

    return this.http.get<UserDto>(`${this.baseUrl}getUserInfo`, { params: params }).pipe(
      catchError(err => {
        console.error("User API error:", err);
        return throwError(() => new Error('Failed to fetch user data'));
      })
    );
  }

  public updateUser(userData: UserDto) : Observable<Result<Boolean>> {
    return this.http.post<Result<Boolean>>(`${this.baseUrl}update`, userData);
  }
}
