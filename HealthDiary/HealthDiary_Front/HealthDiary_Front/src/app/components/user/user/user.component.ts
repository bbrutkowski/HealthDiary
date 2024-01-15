import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Subscription, take } from 'rxjs';
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
  public userDto: any;
  public loadError = false;
  public dataLoaded = false;

  public constructor(private userService: UserService){}

  ngOnInit(): void {
    this.getLoggedUserData();
    this.dataLoaded = true;
  }

  ngOnDestroy(): void {
    if (this.userDataSubscription) {
      this.userDataSubscription.unsubscribe();
    }
  }

  private getLoggedUserData() {
    const loggedUser = localStorage.getItem('loggedUser') as string;
    if(loggedUser) {
      const userId = JSON.parse(loggedUser).id;

      this.userDataSubscription = this.userService.getUserById(userId).pipe(take(1)).subscribe(response => {
        if(response.isSuccess){
          this.userDto = response.data;
        }
      }, (error) => {
        this.loadError = true;
        console.log(error.error.errorMessage);
      })
    }   
  }
}
