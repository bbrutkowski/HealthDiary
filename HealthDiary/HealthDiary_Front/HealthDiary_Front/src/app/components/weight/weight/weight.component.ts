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

  ngOnInit(): void {
    this.latestUpdate = this.getLatestWeightUpdate();
  }

  private getLatestWeightUpdate(): Date {
    if (this.weights.length > 0) {
      const sortedWeights = this.weights.sort((a: { creationDate: Date; }, b: { creationDate: Date; }) => new Date(b.creationDate)
        .getTime() - new Date(a.creationDate).getTime());

      return sortedWeights[0]?.creationDate;
    }
  
    return null;
  }
}
