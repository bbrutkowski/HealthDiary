import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, Subscription, catchError, of, take} from 'rxjs';
import { SleepInfoDto } from 'src/app/models/sleep-info-dto';
import { TotalActivityDto } from 'src/app/models/total-activity';
import { WeeklyNutritionDto } from 'src/app/models/weekly-nutrition-dto';
import { WeightDto } from 'src/app/models/weight-dto';
import { ActivityService } from 'src/app/services/activity.service/activity.service';
import { FoodService } from 'src/app/services/food.service/food.service';
import { SleepService } from 'src/app/services/sleep.service/sleep.service';
import { WeightService } from 'src/app/services/weight.service/weight.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit, OnDestroy {
  public weatherContent: string;
  private destroy$ = new Subject<void>();
  public dataLoaded = false;
  public userId: number;
  public userWeights: Array<WeightDto> = [];
  public totalMonthlyActivities: TotalActivityDto;
  public weeklyNutritionDto: WeeklyNutritionDto;
  public lastSleepInfo: SleepInfoDto;
  private weatherSubscription: Subscription;
  public cityName: string;

  public constructor(
    private weightService: WeightService,
    private activityService: ActivityService,
    private foodService: FoodService,
    private sleepService: SleepService) {}

  ngOnInit(): void {
    this.getUserId();
    this.initWeight();
    this.initActivities();
    this.initFood();
    this.initSleep();
  
    this.dataLoaded = true;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.weatherSubscription?.unsubscribe();
  }

  private getUserId(): void {
    this.userId = +localStorage.getItem('userId');
  }

  private initWeight(): void { 
    this.weightService.getUserWeightsByMonth(this.userId).pipe(
        take(1),
        catchError(err => {
          this.handleError(err);
          return of([]); 
        })
    ).subscribe(weights => {
      this.userWeights = weights; 
  });
}

  private initActivities(): void {
    this.activityService.getMonthlyActivityByUserId(this.userId).pipe(
      take(1),
      catchError(err => {
        this.handleError(err);
        return of(new TotalActivityDto)
      })
    ).subscribe(activities => {
      this.totalMonthlyActivities = activities;
    });
  }

  private initFood(): void {
    this.foodService.getWeeklyMealInformationByUserId(this.userId).pipe(
      take(1),
      catchError(err => {
        this.handleError(err);
        return of(new WeeklyNutritionDto)
      })
      ).subscribe(nutrition => {
        this.weeklyNutritionDto = nutrition
      }
    )
  }

  private initSleep(): void {
    this.sleepService.getLastSleepInformationByUserId(this.userId).pipe(
      take(1),
      catchError(err => {
        this.handleError(err);
        return of(new SleepInfoDto)
      })
    ).subscribe(sleep => {
      this.lastSleepInfo = sleep
    });
  }

  private handleError(error: any): void {
    console.log(error.message);
  }
}
