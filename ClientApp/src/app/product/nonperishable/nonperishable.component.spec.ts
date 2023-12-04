import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NonperishableComponent } from './nonperishable.component';

describe('NonperishableComponent', () => {
  let component: NonperishableComponent;
  let fixture: ComponentFixture<NonperishableComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NonperishableComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NonperishableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
