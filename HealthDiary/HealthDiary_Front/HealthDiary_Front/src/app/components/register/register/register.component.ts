import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { passwordMatchValidator } from 'src/app/helpers/password-match-directive/password-match-validator';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit{
  type: string = 'password';
  isText: boolean = false;
  eyeIcon: string = 'fa-eye-slash';
  registerForm!: FormGroup;


  public constructor (private formBuilder: FormBuilder)  {}      
 
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
