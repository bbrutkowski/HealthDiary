import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { MealDto } from 'src/app/models/meal-dto';
import { Result } from 'src/app/models/operation-result';
import { WeeklyNutritionDto } from 'src/app/models/weekly-nutrition-dto';

@Injectable({
  providedIn: 'root'
})
export class FoodService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = 'https://localhost:7241/api/food/';

  public getLastMealInformationByUserId(paramValue: number): Observable<Result<MealDto>> {
    const params = new HttpParams().set('Id', paramValue);
    return this.http.get<Result<MealDto>>(`${this.baseUrl}getMealInfo`, { params: params });
  }

  public getWeeklyMealInformationByUserId(paramValue: number): Observable<Result<WeeklyNutritionDto>> {
    const params = new HttpParams().set('Id', paramValue);
    return this.http.get<Result<WeeklyNutritionDto>>(`${this.baseUrl}getNutritionInfo`, { params: params });
  }
}
