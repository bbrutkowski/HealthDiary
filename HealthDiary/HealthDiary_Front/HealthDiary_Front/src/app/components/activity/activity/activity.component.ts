import { Component, Input, OnInit } from '@angular/core';
import { TotalActivityDto } from 'src/app/models/total-activity';

@Component({
  selector: 'app-activity',
  templateUrl: './activity.component.html',
  styleUrl: './activity.component.css'
})
export class ActivityComponent implements OnInit {
  @Input() monthlyActivities: TotalActivityDto;

  public isLoading: boolean = true;

  ngOnInit(): void {
    this.isLoading = false;  // temporary solution
  }

}
