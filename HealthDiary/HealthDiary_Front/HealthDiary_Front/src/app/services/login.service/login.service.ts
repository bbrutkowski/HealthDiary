import { Observable, catchError, map, of } from "rxjs";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Result } from "../../models/operation-result";
import { UserDto } from "src/app/models/user-dto";

@Injectable({
    providedIn: 'root'
  })
export class LoginService  {

   constructor(private http: HttpClient){}
   private baseUrl: string = 'https://localhost:7241/api/auth/'

   public login(loginData: any): Observable<UserDto | string> {
      return this.http.post<UserDto | string>(`${this.baseUrl}Login`, loginData).pipe(
        map(response => {
          if (typeof response === 'string') throw new Error(response); 
          return response as UserDto; 
        }),
        catchError(error => {
          return of(error.message); 
        })
      );
    }
}