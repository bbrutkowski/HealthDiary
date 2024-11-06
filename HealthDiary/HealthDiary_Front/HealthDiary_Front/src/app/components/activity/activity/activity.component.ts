import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { catchError, of, Subscription, take } from 'rxjs';
import { AddActivityModalComponent } from 'src/app/helpers/add-activity-modal/add-activity-modal/add-activity-modal.component';
import { ActivityCatalog } from 'src/app/models/activity-catalog';
import { TotalActivityDto } from 'src/app/models/total-activity';
import { WeeklyActivity } from 'src/app/models/weekly-activity';
import { ActivityService } from 'src/app/services/activity.service/activity.service';

@Component({
  selector: 'app-activity',
  templateUrl: './activity.component.html',
  styleUrl: './activity.component.css'
})
export class ActivityComponent implements OnInit {
  // @Input() monthlyActivities: TotalActivityDto;

  public isLoading: boolean = true;
  public userId: number;
  public activities: ActivityCatalog;
  public weeklyActivities: Array<WeeklyActivity> = [];
  public chartName: string;

  constructor(
    private activityService: ActivityService,
    private dialog: MatDialog
  ){}

  ngOnInit(): void {
    this.getUserId();
    this.initActivitiesCatalog();
    this.initweeklyActivities();
    this.mapChartData();
    this.isLoading = false; 
  }

  private getUserId(){
    this.userId = +localStorage.getItem('userId');
  }

  private initActivitiesCatalog(): void {
    this.activityService.getActivities(this.userId).pipe(
      take(1),
      catchError(err => {
        this.handleError(err);
        return of(this.activities)
      })
    ).subscribe(activities => {
      this.activities = activities;
    })
  }

  private initweeklyActivities() : void {
    this.activityService.getWeeklyActivities(this.userId).pipe(
      take(1),
      catchError(error => {
        this.handleError(error);
        return of(this.weeklyActivities)
      })
    ).subscribe(weeklyActivities => {
      this.weeklyActivities = weeklyActivities;
      this.chartName = "Weekly activity"
    })
  }

  private mapChartData() : void {
    if(!this.weeklyActivities) return;


  }

  public openAddActivityModal(): void {
    this.activities.userId = this.userId;

    this.dialog.open(AddActivityModalComponent, {
      height: '570px',
      data: {
        activities: this.activities.activities,
        lastUserWeight: this.activities.lastUserWeight,
        userId: this.activities.userId
      }
    });
  }

  private handleError(error: any): void {
    console.log(error.message);
  }
}
