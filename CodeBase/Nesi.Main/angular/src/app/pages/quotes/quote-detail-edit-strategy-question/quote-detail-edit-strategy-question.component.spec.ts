/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { QuoteDetailEditStrategyQuestionComponent } from './quote-detail-edit-strategy-question.component';

describe('QuoteDetailEditStrategyQuestionComponent', () => {
  let component: QuoteDetailEditStrategyQuestionComponent;
  let fixture: ComponentFixture<QuoteDetailEditStrategyQuestionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuoteDetailEditStrategyQuestionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuoteDetailEditStrategyQuestionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
