import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PerishableComponent } from './perishable.component';

describe('PerishableComponent', () => {
  let component: PerishableComponent;
  let fixture: ComponentFixture<PerishableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PerishableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PerishableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
