import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription, of } from 'rxjs';
import { catchError, take } from 'rxjs/operators';
import { UserAuthDto } from 'src/app/models/user-auth-dto';
import { UserDto } from 'src/app/models/user-dto';
import { AuthService } from 'src/app/services/auth.service/auth.service';
import { LoginService } from 'src/app/services/login.service/login.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit, OnDestroy {
  public type: string = 'password';
  private isText: boolean = false;
  public eyeIcon: string = 'fa-eye-slash'
  public loginForm!: FormGroup;
  private loginSubscription: Subscription;
  public loginError = false;
  public messageError: string;

  public constructor(
    private formBuilder: FormBuilder,
    private loginService: LoginService,
    private router: Router,
    private authService: AuthService) 
  {}

  get login(){
    return this.loginForm.controls['login'];
  }

  get password(){
    return this.loginForm.controls['password'];
  }

  ngOnDestroy(): void {
    if (this.loginSubscription) {
      this.loginSubscription.unsubscribe();
    }
  }

  ngOnInit(): void {
    this.loginForm = this.formBuilder.group({
      login: ['', [Validators.required]],
      password: ['', [Validators.required]]
    });

    this.setupFormValueChangesListener();
  }

  public hideShowPassword() : void {
    this.isText = !this.isText;
    this.isText ? this.eyeIcon = 'fa-eye' : this.eyeIcon = 'fa-eye-slash';
    this.isText ? this.type = 'text' : this.type = 'password';   
  }

  public onSubmit(): void {
    if (!this.loginForm.valid) return this.validateForm(this.loginForm);
  
    this.loginService.login(this.loginForm.value).pipe(
      catchError(error => {
        this.messageError = error;    
        return of(this.loginError = true);
      })
    ).subscribe({
      next: (response: UserAuthDto) => {
        if (!response) return this.loginForm.reset();
        let isStored = this.authService.storeLoggedUser(response);
        if (isStored)  this.router.navigate(['startup']);
      }
    });
  }

  private validateForm(formGroup: FormGroup) : void {
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

  private setupFormValueChangesListener(): void {
    this.loginForm.valueChanges.subscribe(() => {
      this.loginError = false;
    });
  }
}
