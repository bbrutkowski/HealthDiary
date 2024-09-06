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

  public constructor(private weightServce: WeightService) {}

  ngOnInit(): void {

  }
}
