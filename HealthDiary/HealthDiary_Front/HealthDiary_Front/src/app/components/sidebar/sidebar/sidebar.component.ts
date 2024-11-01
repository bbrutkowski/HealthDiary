import { Component, OnDestroy, OnInit} from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { Subject, Subscription, of } from 'rxjs';
import { PopupModalComponent } from 'src/app/helpers/popup-modal/popup-modal/popup-modal.component';
import { catchError, takeUntil } from 'rxjs/operators';
import { WeatherService } from 'src/app/services/weather.service/weather.service';
import { WeatherResponseDto } from 'src/app/models/weather-response-dto';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent implements OnInit, OnDestroy  {
  private ngUnsubscribe = new Subject<void>();
  private sidebarSubscription: Subscription = new Subscription(); 
  public userName: string;
  public userRole: string;
  public weatherData: WeatherResponseDto;
  public errorMessage: string;

  constructor(
    private router: Router,
    private dialog: MatDialog,
    private weatherService: WeatherService
  ) {}
    
  public ngOnInit(): void {
    this.getUserPrivileges();  
    this.initWeather();
  }

  ngOnDestroy(): void {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();  
    this.sidebarSubscription.unsubscribe();
  }

  private getUserPrivileges(): void {
    this.userName = localStorage.getItem('name');
    this.userRole = localStorage.getItem('role');
  }

  private initWeather() {
    if (navigator.geolocation){
      navigator.geolocation.getCurrentPosition((position) => {
        const latitude = position.coords.latitude
        const longitude = position.coords.longitude

        this.sidebarSubscription.add(
          this.weatherService.getWeather(latitude, longitude).pipe(
            catchError(err => {
              return of(this.errorMessage = err);
            })
          ).subscribe(weather => {
            this.weatherData = weather
          })
        );       
      })
    }
  }

  public logout(): void {
    const dialogRef = this.openLogoutDialog();

    dialogRef.afterClosed()
      .pipe(takeUntil(this.ngUnsubscribe))
      .subscribe(result => {
        if (result) this.handleLogout();
      });
  }

  private openLogoutDialog(): MatDialogRef<PopupModalComponent> {
    return this.dialog.open(PopupModalComponent, {
      data: {
        modalTitle: 'Logout',
        modalBody: 'Are you sure you want to logout?'
      }
    });
  }

  private handleLogout(): void {
    localStorage.clear();
    this.router.navigate(['login']);
  }

  public openUserProfile():void {
    document.body.classList.add('blurred-background');

    const userRef = this.dialog.open(PopupModalComponent, {
      data: {
        modalTitle: 'User profile',
        modalBody: 'Do you want to go to the user`s profile?'
      }
    });

    userRef.componentInstance.confirmationEvent.subscribe((result: boolean) => {
      if (result) {
        this.router.navigate(['/user']);        
      }
    });

    userRef.afterClosed().subscribe(() => {
      document.body.classList.remove('blurred-background');
    });
  }
}