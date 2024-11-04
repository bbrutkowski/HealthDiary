import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, take, throwError } from 'rxjs';
import { RegisterUserData } from 'src/app/models/login-user-data-dto';
import { UserDto } from 'src/app/models/user-dto';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl: string = 'https://localhost:7241/api/user/'
  private userErrorMessage = 'User API error:'

  constructor(private http: HttpClient) { }

  public register(registerData: RegisterUserData) : Observable<boolean>{
    return this.http.post<boolean>(`${this.baseUrl}register`, registerData).pipe(
      take(1),
      catchError(err => {
        console.error(this.userErrorMessage, err);
        return throwError(() => new Error('Failed to register user'))
      })
    );
  }

  public getUserById(paramValue: number): Observable<UserDto>{
    const params = new HttpParams().set('Id', paramValue);

    return this.http.get<UserDto>(`${this.baseUrl}get-user-info`, { params: params }).pipe(
      catchError(err => {
        console.error(this.userErrorMessage, err);
        return throwError(() => new Error('Failed to fetch user data'));
      })
    );
  }

  public updateUser(userData: UserDto): Observable<boolean> {
    return this.http.post<boolean>(`${this.baseUrl}update`, userData).pipe(
      catchError(err => {
        console.error(this.userErrorMessage, err);
        return throwError(() => new Error('Failed to update user data'));
      })
    );
  }

  public updateUserAvatar(userId: number, avatar: string): Observable<boolean> {
    return this.http.post<boolean>(`${this.baseUrl}update-avatar`, {userId, avatar}).pipe(
      catchError(err => {
        console.error(this.userErrorMessage, err);
        return throwError(() => new Error('Failed to update user avatar'));
      })
    );
  }
}
