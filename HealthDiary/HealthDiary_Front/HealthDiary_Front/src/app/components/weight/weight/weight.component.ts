import { Component, Input, OnInit } from '@angular/core';
import { take } from 'rxjs';
import { WeightDto } from 'src/app/models/weight-dto';
import { WeightService } from 'src/app/services/weight.service/weight.service';

@Component({
  selector: 'app-weight',
  templateUrl: './weight.component.html',
  styleUrl: './weight.component.css'
})
export class WeightComponent implements OnInit {
  @Input() weights: Array<WeightDto>;
  @Input() userId: number;

  public latestUpdate: Date;

  public constructor(private weightServce: WeightService) {}

  ngOnInit(): void {
    this.latestUpdate = this.getLatestWeightUpdate();
    this.initYearlyWeight();
  }

  private getLatestWeightUpdate(): Date {
    if (this.weights.length > 0) {
      const sortedWeights = this.weights.sort((a: { creationDate: Date; }, b: { creationDate: Date; }) => new Date(b.creationDate)
        .getTime() - new Date(a.creationDate).getTime());

      return sortedWeights[0]?.creationDate;
    }
  
    return null;
  }

  private initYearlyWeight(): void {
    this.weightServce.getUserYearlyWeightById(this.userId).pipe(take(1)).subscribe(result => {
      if(result.isSuccess) {
        this.weights = result.data;
      }
    }, err => {
      console.log(err.error.errorMessage);
    })


  }
}
