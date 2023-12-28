import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { take } from 'rxjs/operators';
import { LoginUserDataDto } from 'src/app/models/login-user-data-dto';
import { LoginService } from 'src/app/services/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {
  type: string = 'password';
  isText: boolean = false;
  eyeIcon: string = 'fa-eye-slash'
  loginForm!: FormGroup;

  public constructor(
    private formBuilder: FormBuilder,
    private loginService: LoginService) 
  {}

  get userName(){
    return this.loginForm.controls['userName'];
  }

  get password(){
    return this.loginForm.controls['password'];
  }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      userName: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });
  }

  public hideShowPassword() {
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = 'fa-eye' : this.eyeIcon = 'fa-eye-slash';
    this.isText ? this.type = 'text' : this.type = 'password';   
  }

  public onSubmit(){
    if(!this.loginForm.valid) return this.validateForm(this.loginForm);

    const loginData: LoginUserDataDto = {
      userName: this.loginForm.controls['userName'].value as string,
      password: this.loginForm.controls['password'].value as string
    }

    this.loginService.login(loginData).pipe(take(1)).subscribe(result => {
      
    })
  }

  private validateForm(formGroup: FormGroup){
    Object.keys(formGroup.controls).forEach(field => {
      const control = formGroup.get(field);
      if(control instanceof FormControl) {
        control.markAsDirty({onlySelf: true});
      }
      else if (control instanceof FormGroup){
        this.validateForm(control)
      }
    })
  }
  
}
