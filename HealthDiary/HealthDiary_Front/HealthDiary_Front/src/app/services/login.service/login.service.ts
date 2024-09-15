import { Observable, catchError, take, throwError } from "rxjs";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { UserDto } from "src/app/models/user-dto";
import { UserAuthDto } from "src/app/models/user-auth-dto";

@Injectable({
    providedIn: 'root'
  })
export class LoginService  {

   constructor(private http: HttpClient){}
   private baseUrl: string = 'https://localhost:7241/api/auth/'

   public login(loginData: any): Observable<UserAuthDto> {
    return this.http.post<UserAuthDto>(`${this.baseUrl}Login`, loginData).pipe(
      take(1),
      catchError(err => {
        console.error("Auth API error:", err);
        return throwError(() => new Error('Failed to login user'));
      })
    );
  }
}