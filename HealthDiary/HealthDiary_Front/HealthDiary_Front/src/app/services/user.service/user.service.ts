import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, take, throwError } from 'rxjs';
import { UserDto } from 'src/app/models/user-dto';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl: string = 'https://localhost:7241/api/user/'

  constructor(private http: HttpClient) { }

  public register(registerData: any) : Observable<boolean>{
    return this.http.post<boolean>(`${this.baseUrl}register`, registerData).pipe(
      take(1),
      catchError(err => {
        console.error("User API error:", err);
        return throwError(() => new Error('Failed to register user'))
      })
    );
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

  public updateUser(userData: UserDto): Observable<boolean> {
    return this.http.post<boolean>(`${this.baseUrl}update`, userData).pipe(
      catchError(err => {
        console.error("Update API error:", err);
        return throwError(() => new Error('Failed to update user data'));
      })
    );
  }
}
