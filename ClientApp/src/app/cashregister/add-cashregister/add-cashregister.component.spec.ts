import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCashregisterComponent } from './add-cashregister.component';

describe('AddCashregisterComponent', () => {
  let component: AddCashregisterComponent;
  let fixture: ComponentFixture<AddCashregisterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddCashregisterComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddCashregisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
