import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CashregisterComponent } from './cashregister.component';

describe('CashregisterComponent', () => {
  let component: CashregisterComponent;
  let fixture: ComponentFixture<CashregisterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ CashregisterComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CashregisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
