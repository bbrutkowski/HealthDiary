import { Observable } from "rxjs";
import { LoginUserDataDto } from "../models/login-user-data-dto";
import { RequestHelperService } from "./request-helper/request-helper.service";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Injectable({
    providedIn: 'root'
  })
export class LoginService extends RequestHelperService {

    constructor(http: HttpClient) {
        super(http);
    }

    protected override getApiRoute(): string {
        return "User"
    }

    public login(data: LoginUserDataDto) : Observable<Boolean>{
        return this.createPostRequest<Boolean>('Login', {data: data})
    }
    
}