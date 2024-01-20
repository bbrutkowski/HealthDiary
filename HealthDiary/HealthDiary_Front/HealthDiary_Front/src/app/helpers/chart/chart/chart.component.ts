import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import * as Highcharts from 'highcharts';
import { WeightDto } from 'src/app/models/weight-dto';

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css'],
})
export class ChartComponent implements OnChanges { 
  @Input()
  set userWeights(value: WeightDto[]) {
    this._userWeights = value;
    this.updateChart();
  }

  private _userWeights: WeightDto[] = [];

  public Highcharts: typeof Highcharts = Highcharts;
  public chartConstructor: string = 'chart';
  public chartOptions: any = {
    series: [{
      type: 'line',
      data: [],
    }],
  };
  public updateFlag: boolean = false;
  public oneToOneFlag: boolean = true;
  public runOutsideAngular: boolean = false;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['userWeights']) {
      this.updateChart();
    }
  }

  private updateChart(): void {
    this.chartOptions.series[0].data = this._userWeights.map((weight) => ({
      x: weight.creationDate.getTime(),
      y: weight.value,
    }));
    this.updateFlag = true;
  }
}