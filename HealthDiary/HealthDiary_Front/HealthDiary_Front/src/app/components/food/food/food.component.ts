import { Component, Input } from '@angular/core';
import { MealDto } from 'src/app/models/meal-dto';

@Component({
  selector: 'app-food',
  templateUrl: './food.component.html',
  styleUrl: './food.component.css'
})
export class FoodComponent {
  @Input() mealDto: MealDto;

}
