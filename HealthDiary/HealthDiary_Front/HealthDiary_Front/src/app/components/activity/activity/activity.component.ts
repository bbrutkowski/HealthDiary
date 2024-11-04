import { Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AddActivityModalComponent } from 'src/app/helpers/add-activity-modal/add-activity-modal/add-activity-modal.component';
import { TotalActivityDto } from 'src/app/models/total-activity';

@Component({
  selector: 'app-activity',
  templateUrl: './activity.component.html',
  styleUrl: './activity.component.css'
})
export class ActivityComponent implements OnInit {
  @Input() monthlyActivities: TotalActivityDto;

  public isLoading: boolean = true;

  constructor(
    private dialog: MatDialog
  ){}

  ngOnInit(): void {
    this.isLoading = false;  // temporary solution
  }

  public openAddActivityModal(): void{
    const dialogRef = this.dialog.open(AddActivityModalComponent, {
      height: '450px'
    });

  }

}
