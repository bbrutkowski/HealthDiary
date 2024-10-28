import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, ValidationErrors, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Subscription, catchError, of, take, timer } from 'rxjs';
import { UserDto } from 'src/app/models/user-dto';
import { UserService } from 'src/app/services/user.service/user.service';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit, OnDestroy {
  public userProfile!: FormGroup;
  private userDataSubscription: Subscription;
  public userDto: UserDto;
  public isLoadError = false;
  public isDataLoaded = false;
  public isUpdateSuccessful = false;
  public isUpdateError = false; 
  private defaultAvatar: string = 'assets/default-avatar.png';
  public imageError = false;
  public errorMessage: string;
  private userId: number;

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
    const userIdString = localStorage.getItem('userId');
    this.userId = userIdString ? parseInt(userIdString) : null;
  
    if (this.userId > 0) {
      this.userDataSubscription = this.userService.getUserById(this.userId).pipe(
        take(1),
        catchError(() => {
          return of(new UserDto());
        })
      ).subscribe(userInfo => {
        this.userDto = userInfo;
        this.populateForm();
        this.isDataLoaded = true;
      });
    }
  }

private populateForm(): void {
  const genderMap: { [key: string]: number } = {
    Male: 0,
    Female: 1
  };

  this.userProfile.patchValue({
    id: this.userDto.id,
    name: this.userDto.name,
    surname: this.userDto.surname,
    email: this.userDto.email,
    birthDate: this.formatDate(this.userDto.dateOfBirth),
    gender: genderMap[this.userDto.gender],
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
  const formData = this.userProfile.value;
  
  this.userService.updateUser(formData).pipe(
    take(1),
    catchError(() => {
      return of(this.isUpdateSuccessful = false);
    })
  ).subscribe(result => {
    if (!result) return this.isUpdateSuccessful = false;
    else {
      this.isUpdateSuccessful = result;
      timer(3000).subscribe(() => this.router.navigate(['dashboard']));
    }
  });
}

private formatDate(date: any): string {
  const dateObj = (date instanceof Date) ? date : new Date(date);
  return dateObj.toISOString().split('T')[0]; 
}

public getUserAvatar(): string {
  return this.userDto.avatar ? `data:image/png;base64,${this.userDto.avatar}` : this.defaultAvatar;
}

public onFileSelected(event: Event): void {
  const file = (event.target as HTMLInputElement).files?.[0];
  if (!file) return;

  if (!file.type.startsWith('image/')) {
    alert('Please select a valid image file.');
    return;
  }

  const reader = new FileReader();

  reader.onload = () => {
    const img = new Image();
    img.src = reader.result as string;

    img.onload = () => {
      if (img.width <= 500 && img.height <= 500) {
        const base64String = (reader.result as string).split(',')[1];

        this.userDataSubscription = this.userService.updateUserAvatar(this.userId, base64String).pipe(
            catchError(err => {
              this.errorMessage = err;
              return of(this.imageError = true);
            })
          ).subscribe({
            next: (response: boolean) => {
              if (!response) {
                this.imageError = true;
                return;
              }
              this.userDto.avatar = base64String;
            }
          })
      } else {
        alert('The selected photo should have maximum dimensions of 500x500 px.');
      }
    };

    img.onerror = () => {
      alert('The selected image failed to load.');
    };
  };

  reader.onerror = () => {
    alert('There was a problem reading the file. Please try again.');
  };

  reader.readAsDataURL(file);
}
}