import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Subject, Subscription, catchError, finalize, of, take, tap, timer } from 'rxjs';
import { BmiDto } from 'src/app/models/bmi-dto';
import { WeightDto } from 'src/app/models/weight-dto';
import { WeightGoalDto } from 'src/app/models/weight-goal-dto';
import { WeightService } from 'src/app/services/weight.service/weight.service';

@Component({
  selector: 'app-weight',
  templateUrl: './weight.component.html',
  styleUrl: './weight.component.css'
})
export class WeightComponent implements OnInit, OnDestroy { 
  private destroy$ = new Subject<void>();
  public userId: number;
  public isWeightGoalSet: boolean = false;
  public isLoading: boolean = true;
  public weightGoal: WeightGoalDto;
  public weightGoalForm!: FormGroup;
  private weightSubscription: Subscription = new Subscription(); 
  public isWeightGoalError = false;
  public isSavingGoal = false;
  public showSuccessCheckIcon = false;
  public showGoalForm = false;
  public userWeights: Array<WeightDto> = [];
  public chartName: string;
  public chartHeight: Number;
  public bmiInfo: BmiDto
  public height: Number;
  public weight: Number;

  constructor(
    private weightService: WeightService,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {
    this.getUserId();
    this.initWeightGoal()
    this.initWeightGoalForm();
    this.getWeightGoal();
    this.getYearlyWeight();
    this.initBMI();

    this.isLoading = false;
  }
  
  private getUserId() {
    this.userId = +localStorage.getItem('userId');
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    this.weightSubscription.unsubscribe();
  }

  private getWeightGoal() {
    if (!this.userId) return console.error("No user Id");
  
    this.weightSubscription.add(
      this.weightService.getWeightGoal(this.userId).pipe(
        take(1),
        catchError(err => {
          this.handleError(err);
          return of(this.weightGoal);
        })
      ).subscribe(weightGoal => {
        this.weightGoal = weightGoal;
        if (this.weightGoal.isSet) {
          this.isWeightGoalSet = true; 
        }
      })
    );
  }
  
  private initWeightGoal() {
    this.weightGoal = {
      userId: this.userId,
      isSet: false,
      currentWeight: 0,
      targetWeight: 0,
      creationDate: new Date(),
      targetDate: new Date()
    };
  }

  public setWeightGoal() {
    this.showGoalForm = true;
  }

  private initWeightGoalForm() {
    this.weightGoalForm = this.fb.group({
      userId: [this.userId],
      currentWeight: [this.weightGoal.currentWeight],
      targetWeight: [this.weightGoal.targetWeight],
      creationDate: [this.weightGoal.creationDate || this.getTodayDate()],
      targetDate: [this.weightGoal.targetDate]
    });
  }

  private getTodayDate(): string {
    const today = new Date();
    return today.toISOString().split('T')[0]; 
  }

  public saveWeightGoal() {
    if (this.weightGoalForm.valid) {
      const weightGoalData = this.weightGoalForm.value;
  
      this.isSavingGoal = true;
  
      this.weightSubscription.add(
        this.weightService.saveWeightGoal(weightGoalData).pipe(
          tap(() => this.isSavingGoal = false), 
          tap(() => this.showSuccessCheckIcon = true), 
          catchError(() => {
            this.isWeightGoalError = true;
            return of(false);
          })
        ).subscribe({
          next: (response: boolean) => {
            if (response === false) return;

            timer(2000).subscribe(() => {
              this.showSuccessCheckIcon = false; 
              this.getWeightGoal(); 
              tap(() => this.isWeightGoalSet = true)
            });
          }
        })
      );
    }
  }

  private getYearlyWeight(): void {
    this.weightSubscription.add(
      this.weightService.getYearlyWeight(this.userId).pipe(
        take(1),
        catchError(err => {
          this.handleError(err);
          return of([])
        })
      ).subscribe(weights => {
        this.chartName = "Yearly weight"
        this.chartHeight = 585
        this.userWeights = weights     
      })
    );
  }

  private initBMI(): void {
    this.weightSubscription.add(
      this.weightService.getBMI(this.userId).pipe(
        take(1),
        catchError(err => {
          this.handleError(err);
          return of(null)
        })
      ).subscribe(bmi => {
        this.bmiInfo = bmi
      })
    );
  }

  private handleError(error: any): void {
    console.log(error.message);
  }
}
 