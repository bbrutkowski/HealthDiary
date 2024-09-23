import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HealthPreviewComponent } from './health-preview.component';

describe('HealthPreviewComponent', () => {
  let component: HealthPreviewComponent;
  let fixture: ComponentFixture<HealthPreviewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HealthPreviewComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HealthPreviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
