import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import * as Highcharts from 'highcharts';
import { WeightDto } from 'src/app/models/weight-dto';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css'],
})
export class ChartComponent implements OnChanges {
  @Input() weights: any; 

  public Highcharts: typeof Highcharts = Highcharts;
  public chartConstructor: string = 'chart';
  public chartOptions: any;
  public updateFlag: boolean = false;
  public oneToOneFlag: boolean = true;

  ngOnChanges(changes: SimpleChanges): void {
    const weightsChange = changes['weights'];

    if (weightsChange && weightsChange.currentValue) {
      this.weights = weightsChange.currentValue;
      this.updateChart();
    }
  }

  private updateChart(): void {
    const chartData = this.weights['$values'].map((data: { creationDate: string | number | Date; value: number; }) => ({
      x: new Date(data.creationDate).getDate(),
      y: data.value,
    }));
  
    this.chartOptions = {
      series: [{
        type: 'line',
        data: chartData
      }],
      title: {
        text: 'Weights',
      },
      subtitle: {
        text: 'Weight for the current month',
      },
      legend: {
        enabled: false
      },
      chart: {
        width: null, 
      },
    }
  
    this.updateFlag = true;
  }
}
