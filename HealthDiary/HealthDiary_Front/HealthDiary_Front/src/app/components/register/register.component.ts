import { Component } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  public registerForm = this.formBuilder.group({
    userName: ['', [Validators.minLength(1), Validators.required], Validators.pattern(/^[a-zA-Z0-9@_\-]+$/)],
    password: ['', [Validators.required]],
    confirmPassword: ['', [Validators.required]]
  })

  public constructor(private formBuilder: FormBuilder){}

  get userName(){
    return this.registerForm.controls['userName'];
  }

  get password(){
    return this.registerForm.controls['password'];
  }

  get confirmPassword(){
    return this.registerForm.controls['confirmPassword'];
  }

}
