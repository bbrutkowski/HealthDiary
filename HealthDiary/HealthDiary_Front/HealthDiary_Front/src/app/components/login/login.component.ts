import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

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

  public constructor (private formBuilder: FormBuilder){}

  get userName(){
    return this.loginForm.controls['userName']
  }

  get password(){
    return this.loginForm.controls['password']
  }

}
