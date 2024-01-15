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
   private baseUrl: string = 'https://localhost:7241/api/Auth/'

   public login(loginData: any) : Observable<OperationResult<Boolean>>{
      return this.http.post<OperationResult<Boolean>>(`${this.baseUrl}Login`, loginData)
   }
    
}