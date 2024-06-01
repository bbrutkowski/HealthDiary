import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription, take, timer } from 'rxjs';
import { UserDto } from 'src/app/models/user-dto';
import { UserService } from 'src/app/services/user.service/user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit, OnDestroy {
  public userProfile!: FormGroup;
  private userDataSubscription: Subscription | undefined;
  public userDto: UserDto;
  public isLoadError = false;
  public isDataLoaded = false;
  public isUpdateSuccessful = false;
  public isUpdateError = false; 

  public constructor(private fb: FormBuilder, 
    private userService: UserService,
    private router: Router) {}

  ngOnInit(): void {
    this.getLoggedUserData();
    this.createForm();
  }

  ngOnDestroy(): void {
    if (this.userDataSubscription) {
      this.userDataSubscription.unsubscribe();
    }
  }
  
  private createForm(): void {
    this.userProfile = this.fb.group({
      id: [''],
      name: [''], 
      surname: [''],
      email: [''],
      birthDate: [''],
      phoneNumber: ['', [ this.onlyDigitsValidator ]],
      gender: [''],
      country: [''],
      city: [''],
      street: [''],
      buildingNumber: ['', Validators.min(0)],
      apartmentNumber: ['', Validators.min(0)],
      postalCode: [''] 
    });
  }

  private getLoggedUserData(): void {
    const userId = localStorage.getItem('userId') as unknown as number;
    if(userId) {

      this.userDataSubscription = this.userService.getUserById(userId).pipe(take(1)).subscribe(response => {
        if(response.isSuccess){
          this.userDto = response.data as UserDto;
          this.populateForm();
          this.isDataLoaded = true;
        }
      }, (error) => {
        this.isLoadError = true;
        console.log(error.error.errorMessage);
      })
    }   
  }

  private populateForm(): void {
    this.userProfile.patchValue({
      id: this.userDto.id,
      name: this.userDto.name,
      surname: this.userDto.surname,
      email: this.userDto.email,
      birthDate: this.userDto.birthDate,
      gender: this.userDto.gender,
      phoneNumber: this.userDto.phoneNumber,
      country: this.userDto.address?.country,
      city: this.userDto.address?.city,
      street: this.userDto.address?.street,
      buildingNumber: this.userDto.address?.buildingNumber,
      apartmentNumber: this.userDto.address?.apartmentNumber,
      postalCode: this.userDto.address?.postalCode
    });
  }

  private onlyDigitsValidator(control: FormControl) : ValidationErrors {
    const value = control.value;

    if (value && isNaN(value)) return { 'onlyDigits': true };
    return { 'onlyDigits': false };
  } 

  public onSaved(): void { 
    const formData = this.userProfile.value as UserDto
    this.userService.updateUser(formData).pipe(take(1)).subscribe(result => {
      if(result.isSuccess){
        this.isUpdateSuccessful = true;
        timer(3000).subscribe(() => {
          this.router.navigate(['dashboard']);
        })    
      }
    }, (err) => {
      this.isUpdateError = true;
    })
  }
}
