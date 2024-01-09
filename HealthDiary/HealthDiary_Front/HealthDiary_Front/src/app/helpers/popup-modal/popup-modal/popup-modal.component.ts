import { Component, EventEmitter, Inject} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-popup-modal',
  templateUrl: './popup-modal.component.html',
  styleUrl: './popup-modal.component.css'
})
export class PopupModalComponent {
  public modalBody?: string; 
  public modalTitle?: string;

  public confirmationEvent: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
              private dialogRef: MatDialogRef<any>) {
    this.modalBody = data.modalBody;
    this.modalTitle = data.modalTitle;
  }

  public cancel(): void {
    this.dialogRef.close(false);
  }

  public confirm(): void {
    this.confirmationEvent.emit(true);
    this.dialogRef.close(true);
  }
}




