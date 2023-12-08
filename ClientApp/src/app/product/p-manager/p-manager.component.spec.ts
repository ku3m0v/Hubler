import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PManagerComponent } from './p-manager.component';

describe('PManagerComponent', () => {
  let component: PManagerComponent;
  let fixture: ComponentFixture<PManagerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PManagerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
