import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import * as Highcharts from 'highcharts';
import { MealDto } from 'src/app/models/meal-dto';
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

  public weightsChartOprions: any;
  public mealInfoChartOprions: any;
  public lastSleepChartOprions: any;

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
    const chartData = this.weights.map((data: { creationDate: Date; value: number; }) => ({
      x: new Date(data.creationDate).getDate(),
      y: data.value,
    })).sort((a, b) => a.x - b.x);

    this.weightsChartOprions = {
      animationEnabled: true,
      title: {
        text: "Monthly weight",
        fontFamily: "'Poppins', sans-serif",
        fontStyle: "Bold"
      },
      height: 340,
      data: [{
        type: 'splineArea',
        color: '#A7C4DC',
        xValueFormatString: 'Weight',
        dataPoints: chartData
      }]
    }	
  }

  private updateMealInfoChart(): void {
    if(this.weeklyNutrition == null) return;
    this.mealInfoChartOprions = {
      title:{
        text: "Nutritional values",
        fontFamily: "'Poppins', sans-serif",
        fontStyle: "Bold"
      },
      height: 340,
      animationEnabled: true,
      axisY: {
        includeZero: true,
        suffix: "g"
      },
      data: [{
        type: "bar",
        indexLabel: "{y}",
        yValueFormatString: "###g",
        dataPoints: [
          { label: "Kcal", y: this.weeklyNutrition.kcal },
          { label: "Protein", y: this.weeklyNutrition.protein },
          { label: "Fat", y: this.weeklyNutrition.fat },
          { label: "Carbohydrates", y: this.weeklyNutrition.carbohydrates, },         
        ]
      }]
    }	 
  }

  private updateLastSleepInfoChart(): void {
    if(this.lastSleepInfo == null) return;
    this.lastSleepChartOprions = {
      animationEnabled: true,
      height: 340,
      title:{
      text: null
      },
      data: [{
      type: "doughnut",
      yValueFormatString: "##.##'h'",
      indexLabel: "{name}",
      dataPoints: [
        { y: this.lastSleepInfo.sleepTime, name: 'Last sleep time' },       
      ]
      }]
    }	
  }
}
