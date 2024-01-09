import { Component, ElementRef, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-popup-modal',
  templateUrl: './popup-modal.component.html',
  styleUrl: './popup-modal.component.css'
})
export class PopupModalComponent {
  @Input() public modalBody?: string; 
  @Input() public modalTitle?: string;

  @Output() closeEvent = new EventEmitter();
  @Output() submitEvent = new EventEmitter();

  public constructor(private elementRef: ElementRef) {}

  close(){
    this.elementRef.nativeElement.remove();
    this.closeEvent.emit();
  }

  submit(){
    this.elementRef.nativeElement.remove();
    this.submitEvent.emit();
  }

}




