import { Component, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { Subject, interval, switchMap, take, timer } from 'rxjs';
import { SleepInfoDto } from 'src/app/models/sleep-info-dto';
import { TotalActivityDto } from 'src/app/models/total-activity';
import { WeatherDto } from 'src/app/models/weather-dto';
import { WeeklyNutritionDto } from 'src/app/models/weekly-nutrition-dto';
import { WeightDto } from 'src/app/models/weight-dto';
import { ActivityService } from 'src/app/services/activity.service/activity.service';
import { AuthService } from 'src/app/services/auth.service/auth.service';
import { FoodService } from 'src/app/services/food.service/food.service';
import { SleepService } from 'src/app/services/sleep.service/sleep.service';
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
  public userId: number;
  public userWeights: Array<WeightDto> = [];
  public totalMonthlyActivities: TotalActivityDto;
  public weeklyNutritionDto: WeeklyNutritionDto;
  public lastSleepInfo: SleepInfoDto;

  public constructor(
    private authService: AuthService,
    private weatherService: WeatherService,
    private weightService: WeightService,
    private activityService: ActivityService,
    private foodService: FoodService,
    private sleepService: SleepService) {}

  ngOnInit(): void {
    this.authService.storeToken();
    this.getUserId();
    this.initWeather();
    this.initWeight();
    this.initActivities();
    this.initFood();
    this.initSleep();
  
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
    this.foodService.getWeeklyMealInformationByUserId(this.userId).pipe(take(1)).subscribe(result => {
      if(result.isSuccess) {
        this.weeklyNutritionDto = result.data
      }
    }, err => this.handleError(err));
  }

  private initSleep(): void {
    this.sleepService.getLastSleepInformationByUserId(this.userId).pipe(take(1)).subscribe(result => {
      if(result.isSuccess) {
        this.lastSleepInfo = result.data
      }
    }, err => this.handleError(err));
  }

  private handleError(error: any): void {
    console.log(error.error.errorMessage);
  }
}
