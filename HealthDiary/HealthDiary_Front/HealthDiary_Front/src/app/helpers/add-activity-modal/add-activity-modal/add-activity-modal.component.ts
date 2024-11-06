import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { catchError, delay, of, Subscription, switchMap, take, tap } from 'rxjs';
import { PhysicalActivity } from 'src/app/models/physical-activity';
import { ActivityService } from 'src/app/services/activity.service/activity.service';

@Component({
  selector: 'app-add-activity-modal',
  templateUrl: './add-activity-modal.component.html',
  styleUrl: './add-activity-modal.component.css'
})
export class AddActivityModalComponent implements OnInit {
  public activityForm!: FormGroup;
  public activityOptions: PhysicalActivity[] = [];
  public caloriesBurned: number = 0;
  public isSaving: boolean = false;
  public errorMessage: string | null = null;
  private saveSubscription: Subscription = new Subscription(); 

  constructor(
    private activityService: ActivityService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<AddActivityModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { activities: PhysicalActivity[], lastUserWeight: number, userId: number }
  ){}

  ngOnInit(): void {
    this.initForm();
    this.onFormChanges()
  }

  private initForm(): void {
    this.activityForm = this.fb.group({
      id: this.data.userId,
      activity: ['', Validators.required],
      time: [0, [Validators.required, Validators.min(0)]],
      distance: [{ value: 0, disabled: true }, Validators.min(0)],
      calories: this.caloriesBurned,
      date: [new Date().toISOString().split('T')[0], Validators.required]
    });
  }

  private onFormChanges(): void {
    this.activityForm.valueChanges.subscribe(() => {
      this.calculateCaloriesBurned();
    });
  }

  public onActivityChange(): void {
    const selectedActivity = this.activityForm.value.activity;

    if (selectedActivity && selectedActivity.averageSpeed) {
      this.activityForm.get('distance')?.enable(); 
    } else {
      this.activityForm.get('distance')?.disable(); 
      this.activityForm.get('distance')?.setValue(0); 
    }
  }

  private calculateCaloriesBurned(): void {
    const selectedActivity = this.activityForm.value.activity;
    const time = this.activityForm.value.time;
    const distance = this.activityForm.value.distance;
  
    if (selectedActivity.averageSpeed && distance > 0) {
      const estimatedTime = distance / selectedActivity.averageSpeed;
      this.caloriesBurned = selectedActivity.met * this.data.lastUserWeight * estimatedTime;
    } else {
      this.caloriesBurned = selectedActivity.met * this.data.lastUserWeight * time;
    }
  }

  public onSubmit(): void {
    if (this.activityForm.invalid) return;

    const activityData = this.activityForm.getRawValue(); 

    const dataToSave = {
      id: this.data.userId,
      name: activityData.activity.name,
      time: activityData.time,
      distance : activityData.distance,
      date: activityData.date,
      calories: this.caloriesBurned   
    }

    this.isSaving = true;

    this.saveSubscription = this.activityService.saveActivity(dataToSave)
      .pipe(
        take(1), 
        catchError(error => {
          this.errorMessage = error.message;
          return of(false); 
        }),
        tap((response: boolean) => {
          if (!response) this.errorMessage = 'Saving failed. Please try again';
        }),
        switchMap((response: boolean) => 
          response ? of(true).pipe(delay(2000)) : of(false)
        )
      )
      .subscribe({
        next: (response: boolean) => {
          if (response) this.dialogRef.close(); 
        },
        complete: () => {
          if (this.saveSubscription) {
            this.saveSubscription.unsubscribe(); 
          }
        }
      });
  } 

  public onCancel(): void {
    this.dialogRef.close()
  }
}
