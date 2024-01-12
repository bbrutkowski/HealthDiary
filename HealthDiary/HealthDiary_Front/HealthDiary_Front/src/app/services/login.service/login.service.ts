import { Observable } from "rxjs";
import { RegisterUserData } from "../../models/login-user-data-dto";
import { RequestHelperService } from "../request-helper/request-helper.service";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { OperationResult } from "../../models/operation-result";

@Injectable({
    providedIn: 'root'
  })
export class LoginService  {

   constructor(private http: HttpClient){}
   private baseUrl: string = 'https://localhost:7241/api/User/'

    // protected override getApiRoute(): string {
    //     return "User"
    // }

    // public login(data: LoginUserDataDto) : Observable<Boolean>{
    //     return this.createPostRequest<Boolean>('Login', {data: data})
    // }

   public login(loginData: any) : Observable<OperationResult<Boolean>>{
      return this.http.post<OperationResult<Boolean>>(`${this.baseUrl}Login`, loginData)
   }

   public register(registerData: any) : Observable<OperationResult<Boolean>>{
      return this.http.post<OperationResult<Boolean>>(`${this.baseUrl}Register`, registerData)
   }
    
}