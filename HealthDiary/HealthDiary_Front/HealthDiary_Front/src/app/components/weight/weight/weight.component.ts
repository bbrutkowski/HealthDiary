import { Component, Input, OnInit } from '@angular/core';
import { WeightDto } from 'src/app/models/weight-dto';

@Component({
  selector: 'app-weight',
  templateUrl: './weight.component.html',
  styleUrl: './weight.component.css'
})
export class WeightComponent implements OnInit {
  @Input() weights: Array<WeightDto>;

  public latestUpdate: Date;
  public canUpdateWeight = false;

  ngOnInit(): void {
    this.latestUpdate = this.getLatestWeightUpdate();
    this.isAbleToUpdate();
  }

  private getLatestWeightUpdate(): Date {
    if (this.weights.length > 0) {
      const sortedWeights = this.weights.sort((a: { creationDate: Date; }, b: { creationDate: Date; }) => new Date(b.creationDate)
        .getTime() - new Date(a.creationDate).getTime());

      return sortedWeights[0]?.creationDate;
    }
  
    return null;
  }

  private isAbleToUpdate(): void {
    if(this.weights.length === 0) return;

    const currentDate: string = new Date().toLocaleString();
    const latestUpdateDate: Date = new Date(this.latestUpdate); 
    const latestUpdateDateLocale: string = latestUpdateDate.toLocaleString();
    this.canUpdateWeight = latestUpdateDateLocale !== currentDate;
  }

}
