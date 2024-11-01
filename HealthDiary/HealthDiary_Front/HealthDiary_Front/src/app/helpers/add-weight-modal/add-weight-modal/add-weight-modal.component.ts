import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { filter, Subject, takeUntil } from 'rxjs';
import { WeightService } from 'src/app/services/weight.service/weight.service';

@Component({
  selector: 'app-add-weight-modal',
  templateUrl: './add-weight-modal.component.html',
  styleUrl: './add-weight-modal.component.css'
})
export class AddWeightModalComponent implements OnInit, OnDestroy {
  public weightForm!: FormGroup;
  private unsubscribe$ = new Subject<void>();

  constructor(
    private weightService: WeightService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<AddWeightModalComponent>
  ){}

  ngOnDestroy(): void {
    this.unsubscribe$.next();
    this.unsubscribe$.complete();
  }

  ngOnInit(): void {
    this.initForm();
  }

  public initForm(){
    this.weightForm = this.fb.group({
      id: localStorage.getItem("userId"),
      weight: [null, Validators.required],
      date: [new Date().toISOString().split('T')[0], Validators.required]
    });
  }

  public onSave(){
    if (this.weightForm.valid) {
      const formData = this.weightForm.value;

      this.weightService.addWeight(formData).pipe(
        takeUntil(this.unsubscribe$),
        filter(response => !!response) 
      )
      .subscribe({
        next: (response: boolean) => {
          if(!response){
            // TODO: error message
          }
          this.dialogRef.close(formData);
          
        }
      });
    }
  }

  public onCancel() {
    this.dialogRef.close(); 
  }
}
