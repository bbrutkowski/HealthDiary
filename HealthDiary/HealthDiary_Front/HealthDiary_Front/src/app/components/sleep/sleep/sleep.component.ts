import { Component, Input } from '@angular/core';
import { SleepInfoDto } from 'src/app/models/sleep-info-dto';

@Component({
  selector: 'app-sleep',
  templateUrl: './sleep.component.html',
  styleUrl: './sleep.component.css'
})
export class SleepComponent {
  @Input() lastSleepInfo: SleepInfoDto;

}
