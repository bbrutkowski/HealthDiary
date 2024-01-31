import { Component, Input } from '@angular/core';
import { MealDto } from 'src/app/models/meal-dto';
import { WeeklyNutritionDto } from 'src/app/models/weekly-nutrition-dto';

@Component({
  selector: 'app-food',
  templateUrl: './food.component.html',
  styleUrl: './food.component.css'
})
export class FoodComponent {
  @Input() weeklyNutritionDto: WeeklyNutritionDto;

}
