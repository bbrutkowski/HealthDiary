import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subject, interval, switchMap, take } from 'rxjs';
import { WeatherService } from 'src/app/services/weather.service/weather.service';

@Component({
  selector: 'app-weather-info-bar',
  templateUrl: './weather-info-bar.component.html',
  styleUrl: './weather-info-bar.component.css'
})
export class WeatherInfoBarComponent implements OnInit, OnDestroy {
  public content: any;
  private destroy$ = new Subject<void>();

  public constructor(private weatherService: WeatherService) {}

  ngOnInit(): void {
    this.weatherService.getWeather().pipe(take(1)).subscribe(response => {
      if (response.isSuccess) {
        this.content = response.data;
      }
    });

    interval(5 * 60 * 1000)  
      .pipe(
        switchMap(() => this.weatherService.getWeather().pipe(take(1)))
      )
      .subscribe(response => {
        if (response.isSuccess) {
          this.content = response.data;
        }
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
