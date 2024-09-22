import { Component, Input } from '@angular/core';
import { TotalActivityDto } from 'src/app/models/total-activity';

@Component({
  selector: 'app-activity',
  templateUrl: './activity.component.html',
  styleUrl: './activity.component.css'
})
export class ActivityComponent {
  @Input() monthlyActivities: TotalActivityDto;

}
