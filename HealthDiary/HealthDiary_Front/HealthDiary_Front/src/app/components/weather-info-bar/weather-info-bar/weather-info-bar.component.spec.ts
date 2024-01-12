import { ComponentFixture, TestBed } from '@angular/core/testing';

import { WeatherInfoBarComponent } from './weather-info-bar.component';

describe('WeatherInfoBarComponent', () => {
  let component: WeatherInfoBarComponent;
  let fixture: ComponentFixture<WeatherInfoBarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [WeatherInfoBarComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(WeatherInfoBarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
