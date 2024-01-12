import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subject, interval, switchMap, take } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service/auth.service';
import { WeatherService } from 'src/app/services/weather.service/weather.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit, OnDestroy {
  public weatherContent: any;
  private destroy$ = new Subject<void>();
  public dataLoaded = false;

  public constructor(
    private authService: AuthService,
    private weatherService: WeatherService) {}

  ngOnInit(): void {
    this.authService.storeToken();
    this.GetWeather();
    this.dataLoaded = true;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private GetWeather() {
    this.weatherService.getWeather().pipe(take(1)).subscribe(response => {
      if (response.isSuccess) {
        this.weatherContent = response.data;
      }
    });

    interval(5 * 60 * 1000)  
      .pipe(
        switchMap(() => this.weatherService.getWeather().pipe(take(1)))
      )
      .subscribe(response => {
        if (response.isSuccess) {
          this.weatherContent = response.data;
        }
      });
  }

}
