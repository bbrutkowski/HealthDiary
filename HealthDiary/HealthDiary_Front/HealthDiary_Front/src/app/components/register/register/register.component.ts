import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { NgToastService } from 'ng-angular-popup';
import { Subscription, delay, take, timer } from 'rxjs';
import { passwordMatchValidator } from 'src/app/helpers/password-match-directive/password-match-validator';
import { RegisterUserData } from 'src/app/models/login-user-data-dto';
import { LoginService } from 'src/app/services/login.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit, OnDestroy{
  public type: string = 'password';
  public isText: boolean = false;
  public eyeIcon: string = 'fa-eye-slash';
  public registerForm!: FormGroup;
  private registerSubscription: Subscription | undefined;
  public registerError = false;
  public registerSuccess = false;
  public errorMessage: string;

  public constructor (private formBuilder: FormBuilder,
                      private loginService: LoginService,
                      private router: Router,
                      ) {}  
                     
  ngOnDestroy(): void {
    if (this.registerSubscription) {
      this.registerSubscription.unsubscribe();
    }
  }
 
  ngOnInit(): void {
    this.registerForm = this.formBuilder.group({
      registerUserName: ['', Validators.required],
      registerEmail: ['', Validators.required],
      registerPassword: ['', Validators.required],
      registerConfirmPassword: ['', Validators.required]
    }, {
      validators: passwordMatchValidator
    })
  }

  public get registerUserName(){
    return this.registerForm.controls['registerUserName'];
  }

  public get registerPassword(){
    return this.registerForm.controls['registerPassword'];
  }

  public get registerConfirmPassword(){
    return this.registerForm.controls['registerConfirmPassword'];
  }

  public get registerEmail() {
    return this.registerForm.controls['registerEmail'];
  }

  public hideShowPassword() {
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = 'fa-eye' : this.eyeIcon = 'fa-eye-slash';
    this.isText ? this.type = 'text' : this.type = 'password'; 
  }

  public onRegister(){
    if(!this.registerForm.valid) return this.validateForm(this.registerForm);

    const registerUserData: RegisterUserData = {
      name: this.registerForm.get('registerUserName')?.value,
      password: this.registerForm.get('registerPassword')?.value,
      email: this.registerForm.get('registerEmail')?.value
    }

    this.registerSubscription = this.loginService.register(registerUserData).pipe(take(1)).subscribe(response => {
      if(response.isSuccess){
        this.registerSuccess = true;
        timer(2000).subscribe(() => {
        this.router.navigate(['login'])
      });
      }
    }, (error) => {
      this.registerError = true;
      this.errorMessage = error.error.errorMessage;
    });
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
