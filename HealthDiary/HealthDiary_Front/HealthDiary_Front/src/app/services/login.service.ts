import { Observable } from "rxjs";
import { RegisterUserData } from "../models/login-user-data-dto";
import { RequestHelperService } from "./request-helper/request-helper.service";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Injectable({
    providedIn: 'root'
  })
export class LoginService  {

   constructor(private http: HttpClient){}
   private baseUrl: string = 'https://localhost:7292/api/User/'

    // protected override getApiRoute(): string {
    //     return "User"
    // }

    // public login(data: LoginUserDataDto) : Observable<Boolean>{
    //     return this.createPostRequest<Boolean>('Login', {data: data})
    // }

   public login(loginData: any) : Observable<any>{
      return this.http.post<any>(`${this.baseUrl}Login`, loginData)
   }

   public register(registerData: any) : Observable<any>{
      return this.http.post<any>(`${this.baseUrl}Register`, registerData)
   }
    
}