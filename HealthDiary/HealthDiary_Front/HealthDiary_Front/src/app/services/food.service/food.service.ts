import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, of, throwError } from 'rxjs';
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
    return this.http.get<Result<MealDto>>(`${this.baseUrl}getLastMealInfo`, { params: params });
  }

  public getWeeklyMealInformationByUserId(paramValue: number): Observable<WeeklyNutritionDto> {
    const params = new HttpParams().set('Id', paramValue);
    
    return this.http.get<WeeklyNutritionDto>(`${this.baseUrl}getNutritionInfo`, { params: params }).pipe(
      catchError(error => {
        console.error("Food API error:", error);
        return throwError(() => new Error('Failed to fetch nutrition info'));
      })
    );
  }
}
