import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import * as Highcharts from 'highcharts';
import { WeightDto } from 'src/app/models/weight-dto';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css'],
})
export class ChartComponent implements OnChanges {
  @Input() weights: Array<WeightDto>; 

  public chartOptions: any;

  ngOnChanges(changes: SimpleChanges): void {
    const weightsChange = changes['weights'];

    if (weightsChange && weightsChange.currentValue) {
      this.weights = weightsChange.currentValue;
      this.updateWeightChart();
    }
  }

  private updateWeightChart(): void {
    const chartData = this.weights.map((data: { creationDate: Date; value: number; }) => ({
      x: new Date(data.creationDate).getDate(),
      y: data.value,
    })).sort((a, b) => a.x - b.x);

    this.chartOptions = {
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
}
