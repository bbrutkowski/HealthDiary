import { Injectable } from '@angular/core';
import { RequestHelperService } from './request-helper/request-helper.service';
import { HttpClient } from '@angular/common/http';
import { LoginUserDataDto } from '../models/login-user-data-dto';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends RequestHelperService {

  protected override getApiRoute(): string {
    return 'UserAuth'
  }

  constructor(http: HttpClient) {
    super(http);
  }

  public checkUserAccess(loginData: LoginUserDataDto): Observable<Boolean>{
    return this.createPostRequest<Boolean>('Login', {loginData: loginData});
  }
}
