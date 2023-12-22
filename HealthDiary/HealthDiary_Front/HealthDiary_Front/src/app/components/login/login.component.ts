import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { take } from 'rxjs/operators';
import { LoginUserDataDto } from 'src/app/models/login-user-data-dto';
import { AuthService } from 'src/app/services/auth.service';
import { LoginService } from 'src/app/services/login-service/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  public loginForm = this.formBuilder.group({
    userName: ['', [Validators.minLength(1), Validators.required]],
    password: ['', [Validators.required]]
  })

  public constructor (
    private formBuilder: FormBuilder,
    private authService: AuthService)
  {}

  public isLogged: Boolean = false;

  get userName(){
    return this.loginForm.controls['userName']
  }

  get password(){
    return this.loginForm.controls['password']
  }

  public login(){
    const loginData: LoginUserDataDto = {
      userName: this.loginForm.controls['userName'].value as string,
      password: this.loginForm.controls['password'].value as string
    }

    this.authService.checkUserAccess(loginData).pipe(take(1)).subscribe(result => {
      this.isLogged = result
    })
  }

}

