import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import * as Highcharts from 'highcharts';
import { MealDto } from 'src/app/models/meal-dto';
import { WeightDto } from 'src/app/models/weight-dto';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css'],
})
export class ChartComponent implements OnChanges {
  @Input() weights: Array<WeightDto>; 
  @Input() meal: MealDto;

  public weightsChartOprions: any;
  public mealInfoChartOprions: any;

  ngOnChanges(changes: SimpleChanges): void {
    const weightsChange = changes['weights'];
    const mealInfo = changes['meal'];

    if (weightsChange && weightsChange.currentValue) {
      this.weights = weightsChange.currentValue;
      this.updateWeightChart();
    }

    if (mealInfo && mealInfo.currentValue) {
      this.meal = mealInfo.currentValue;
      this.updateMealInfoChart();
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
        text: "Monthly Weight"
      },
      data: [{
        type: 'splineArea',
        color: '#A7C4DC',
        xValueFormatString: 'Weight',
        dataPoints: chartData
      }]
    }	
  }

  private updateMealInfoChart(): void {
    this.mealInfoChartOprions = {
      title:{
        text: "Nutritional values"
      },
      animationEnabled: true,
      axisY: {
        includeZero: true,
        suffix: "g"
      },
      data: [{
        type: "bar",
        indexLabel: "{y}",
        yValueFormatString: "#,##g",
        dataPoints: [
          { label: "Protein", y: this.meal.protein },
          { label: "Fat", y: this.meal.fat },
          { label: "Carbohydrates", y: this.meal.carbohydrates }
        ]
      }]
    }	 
  }
}
