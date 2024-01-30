import { Observable } from "rxjs";
import { RegisterUserData } from "../../models/login-user-data-dto";
import { RequestHelperService } from "../request-helper/request-helper.service";
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

   public login(loginData: any) : Observable<Result<UserDto>>{
      return this.http.post<Result<UserDto>>(`${this.baseUrl}Login`, loginData)
   }
    
}