import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import * as Highcharts from 'highcharts';
import { Subject, interval, switchMap, take, timer } from 'rxjs';
import { MealDto } from 'src/app/models/meal-dto';
import { TotalActivityDto } from 'src/app/models/total-activity';
import { WeatherDto } from 'src/app/models/weather-dto';
import { WeightDto } from 'src/app/models/weight-dto';
import { ActivityService } from 'src/app/services/activity.service/activity.service';
import { AuthService } from 'src/app/services/auth.service/auth.service';
import { FoodService } from 'src/app/services/food.service/food.service';
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
  public mealDto: MealDto;

  public constructor(
    private authService: AuthService,
    private weatherService: WeatherService,
    private weightService: WeightService,
    private activityService: ActivityService,
    private foodService: FoodService) {}

  ngOnInit(): void {
    this.authService.storeToken();
    this.getUserId();
    this.initWeather();
    this.initWeight();
    this.initActivities();
    this.initFood();
  
    setTimeout(() => {
      this.dataLoaded = true;
    }, 3000);
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private getUserId(): void {
    this.userId = +localStorage.getItem('userId');
  }

  private initWeather() {
    this.weatherService.getWeather().pipe(take(1)).subscribe(response => {
      if (response.isSuccess) {
        this.weatherContent = response.data;
      }
    }, err => this.handleError(err));

    interval(5 * 60 * 1000)  
      .pipe(
        switchMap(() => this.weatherService.getWeather().pipe(take(1)))
      )
      .subscribe(response => {
        if (response.isSuccess) {
          this.weatherContent = response.data;
        }
      }, err => this.handleError(err));
  }

  private initWeight(): void {
    this.weightService.getUserWeightsByMonth(this.userId).pipe(take(1)).subscribe(result => {
      if(result.isSuccess){
        this.userWeights = result.data;
      }
    }, err => this.handleError(err));  
  }

  private initActivities(): void {
    this.activityService.getMonthlyActivityByUserId(this.userId).pipe(take(1)).subscribe(result => {
      if(result.isSuccess) {
        this.totalMonthlyActivities = result.data;
      }     
    }, err => this.handleError(err))
  }

  private initFood(): void {
    this.foodService.getLastMealInformationByUserId(this.userId).pipe(take(1)).subscribe(result => {
      if(result.isSuccess) {
        this.mealDto = result.data
      }
    }, err => this.handleError(err));
  }

  private handleError(error: any): void {
    console.log(error.error.errorMessage);
  }
}
