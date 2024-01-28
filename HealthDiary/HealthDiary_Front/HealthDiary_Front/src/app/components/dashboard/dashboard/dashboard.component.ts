import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import * as Highcharts from 'highcharts';
import { Subject, interval, switchMap, take, timer } from 'rxjs';
import { TotalActivityDto } from 'src/app/models/total-activity';
import { WeatherDto } from 'src/app/models/weather-dto';
import { WeightDto } from 'src/app/models/weight-dto';
import { ActivityService } from 'src/app/services/activity.service/activity.service';
import { AuthService } from 'src/app/services/auth.service/auth.service';
import { WeatherService } from 'src/app/services/weather.service/weather.service';
import { WeightService } from 'src/app/services/weight.service/weight.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit, OnDestroy {
  public weatherContent: WeatherDto;
  private destroy$ = new Subject<void>();
  public dataLoaded = false;
  private userId: number;
  public userWeights: Array<WeightDto> = [];
  public totalMonthlyActivities: TotalActivityDto;

  public constructor(
    private authService: AuthService,
    private weatherService: WeatherService,
    private weightService: WeightService,
    private activityService: ActivityService) {}

  ngOnInit(): void {
    this.authService.storeToken();
    this.getUserId();
    this.initWeather();
    this.initWeight();
    this.initActivities();


    timer(3000).subscribe(() => {
      this.dataLoaded = true;
    })
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private getUserId(): void {
    this.userId = localStorage.getItem('userId') as unknown as number;
  }

  private initWeather() {
    this.weatherService.getWeather().pipe(take(1)).subscribe(response => {
      if (response.isSuccess) {
        this.weatherContent = response.data;
      }
    }, err => {
      console.log(err.error.errorMessage);
    });

    interval(5 * 60 * 1000)  
      .pipe(
        switchMap(() => this.weatherService.getWeather().pipe(take(1)))
      )
      .subscribe(response => {
        if (response.isSuccess) {
          this.weatherContent = response.data;
        }
      }, err => {
        console.log(err.error.errorMessage);
      });
  }

  private initWeight(): void {
    this.weightService.getUserWeightsByMonth(this.userId).pipe(take(1)).subscribe(result => {
      if(result.isSuccess){
        this.userWeights = result.data;
      }
    }, err => {
      console.log(err.error.errorMessage);
    });  
  }

  private initActivities(): void {
    this.activityService.getMonthlyActivityByUserId(this.userId).pipe(take(1)).subscribe(result => {
      if(result.isSuccess) {
        this.totalMonthlyActivities = result.data;
      }     
    }, err => {
      console.log(err.error.errorMessage);
    })
  }
}
