import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
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
  @Output() weightAdded = new EventEmitter<void>();

  public weightForm!: FormGroup;
  private unsubscribe$ = new Subject<void>();
  public errorMessage: string | null = null;
  public isSaving: boolean;

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
      this.isSaving = true;
      const formData = this.weightForm.value;

      this.weightService.addWeight(formData).pipe(
        takeUntil(this.unsubscribe$),
        filter(response => !!response) 
      )
      .subscribe({
        next: (response: boolean) => {
          if(response){      
          this.dialogRef.close(formData);
          this.weightAdded.emit(); 
          this.closeModalAfterDelay();
        }        
        },
        error: error => {
          this.errorMessage = error.message;
          this.closeModalAfterDelay();
        }
      });
    }
  }

  public onCancel() {
    this.dialogRef.close(); 
  }

  private closeModalAfterDelay(): void {
    setTimeout(() => {
      this.dialogRef.close();
    }, 2000); 
  }
}
