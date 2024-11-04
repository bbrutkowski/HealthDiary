import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { ScaleType } from '@swimlane/ngx-charts';
import { SleepInfoDto } from 'src/app/models/sleep-info-dto';
import { WeeklyNutritionDto } from 'src/app/models/weekly-nutrition-dto';
import { WeightDto } from 'src/app/models/weight-dto';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css'],
})
export class ChartComponent implements OnChanges {
  @Input() weights: Array<WeightDto>; 
  @Input() weeklyNutrition: WeeklyNutritionDto;
  @Input() lastSleepInfo: SleepInfoDto;
  @Input() chartName: string;

  public weightChartData: any[] = [];
  public nutritionChartData: any[] = [];
  public sleepChartData: any[] = [];

  colorScheme = {
    domain: ['#A7C4DC', '#6db85c', '#C7B42C', '#AAAAAA'],
    name: 'customScheme',
    selectable: true,
    group: ScaleType.Ordinal 
  };

  ngOnChanges(changes: SimpleChanges): void {
    const weightsChange = changes['weights'];
    const mealInfo = changes['weeklyNutrition'];
    const lastSleepInfo = changes['lastSleepInfo'];

    if (weightsChange && weightsChange.currentValue) {
      this.weights = weightsChange.currentValue;
      this.updateWeightChart();
    }

    if (mealInfo && mealInfo.currentValue) {
      this.weeklyNutrition = mealInfo.currentValue;
      this.updateMealInfoChart();
    }

    if (lastSleepInfo && lastSleepInfo.currentValue) {
      this.lastSleepInfo = lastSleepInfo.currentValue;
      this.updateLastSleepInfoChart();
    }
  }

  private updateWeightChart(): void {
    if (!this.weights) return;
    this.weightChartData = [
      {
        "name": "Weight",
        "series": this.weights.map(weight => ({
          "name": this.formatDate(new Date(weight.creationDate)),
          "value": weight.value
        }))
      }
    ];
  }

  private formatDate(date: Date): string {
    const day = date.getDate().toString().padStart(2, '0');
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    return `${day}-${month}`;
  }

  private updateMealInfoChart(): void {
    if (!this.weeklyNutrition) return;
    this.nutritionChartData = [
      {
        "name": "Kcal",
        "value": this.weeklyNutrition.kcal
      },
      {
        "name": "Protein",
        "value": this.weeklyNutrition.protein
      },
      {
        "name": "Fat",
        "value": this.weeklyNutrition.fat
      },
      {
        "name": "Carbohydrates",
        "value": this.weeklyNutrition.carbohydrates
      }
    ];
  }

  private updateLastSleepInfoChart(): void {
    if (!this.lastSleepInfo) return;
    this.sleepChartData = [
      {
        "name": "Sleep time",
        "value": this.lastSleepInfo.sleepTime
      }
    ];
  }
}
