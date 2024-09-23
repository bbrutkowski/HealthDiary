import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MonthCounterComponent } from './month-counter.component';

describe('MonthCounterComponent', () => {
  let component: MonthCounterComponent;
  let fixture: ComponentFixture<MonthCounterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MonthCounterComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(MonthCounterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
