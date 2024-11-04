import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, throwError } from 'rxjs';
import { WeeklyNutritionDto } from 'src/app/models/weekly-nutrition-dto';

@Injectable({
  providedIn: 'root'
})
export class FoodService {

  constructor(private http: HttpClient) { }

  private baseUrl: string = 'https://localhost:7241/api/food/';

  public getWeeklyMealInformationByUserId(paramValue: number): Observable<WeeklyNutritionDto> {
    const params = new HttpParams().set('Id', paramValue);
    
    return this.http.get<WeeklyNutritionDto>(`${this.baseUrl}get-nutrition-info`, { params: params }).pipe(
      catchError(error => {
        console.error("Food API error:", error);
        return throwError(() => new Error('Failed to fetch nutrition info'));
      })
    );
  }
}
