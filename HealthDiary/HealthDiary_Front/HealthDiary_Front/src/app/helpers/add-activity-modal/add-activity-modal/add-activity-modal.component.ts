import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-add-activity-modal',
  templateUrl: './add-activity-modal.component.html',
  styleUrl: './add-activity-modal.component.css'
})
export class AddActivityModalComponent implements OnInit {
  activityForm!: FormGroup;
  activityOptions = ['Running', 'Swimming', 'Cycling'];

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<AddActivityModalComponent>
  ){}

  ngOnInit(): void {
    this.initForm();
  }

  private initForm(): void {
    this.activityForm = this.fb.group({
      activity: ['', Validators.required],
      time: [0, [Validators.required, Validators.min(0)]],
      date: [new Date().toISOString().split('T')[0], Validators.required]
    });
  }

  public onSubmit(): void {

  } 

  public onCancel(): void {
    this.dialogRef.close()
  }

}
