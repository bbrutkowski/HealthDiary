import { Component, Input } from '@angular/core';
import { SleepInfoDto } from 'src/app/models/sleep-info-dto';
import { TotalActivityDto } from 'src/app/models/total-activity';
import { WeeklyNutritionDto } from 'src/app/models/weekly-nutrition-dto';
import { WeightDto } from 'src/app/models/weight-dto';

@Component({
  selector: 'app-health-preview',
  templateUrl: './health-preview.component.html',
  styleUrl: './health-preview.component.css'
})
export class HealthPreviewComponent {
  @Input() weights: Array<WeightDto> = [];
  @Input() monthlyActivities: TotalActivityDto;
  @Input() weeklyNutritionDto: WeeklyNutritionDto;
  @Input() lastSleepInfo: SleepInfoDto;
  @Input() chartName: string;
  @Input() chartHeight: Number;


}
