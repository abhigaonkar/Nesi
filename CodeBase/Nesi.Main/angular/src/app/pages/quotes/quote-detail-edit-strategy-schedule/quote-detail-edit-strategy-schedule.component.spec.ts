/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { QuoteDetailEditStrategyScheduleComponent } from './quote-detail-edit-strategy-schedule.component';

describe('QuoteDetailEditStrategyScheduleComponent', () => {
  let component: QuoteDetailEditStrategyScheduleComponent;
  let fixture: ComponentFixture<QuoteDetailEditStrategyScheduleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuoteDetailEditStrategyScheduleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuoteDetailEditStrategyScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
