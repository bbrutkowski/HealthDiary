import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddWeightModalComponent } from './add-weight-modal.component';

describe('AddWeightModalComponent', () => {
  let component: AddWeightModalComponent;
  let fixture: ComponentFixture<AddWeightModalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddWeightModalComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(AddWeightModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
