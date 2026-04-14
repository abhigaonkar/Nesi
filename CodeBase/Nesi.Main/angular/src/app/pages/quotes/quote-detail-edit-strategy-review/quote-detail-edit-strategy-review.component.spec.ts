/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { QuoteDetailEditStrategyReviewComponent } from './quote-detail-edit-strategy-review.component';

describe('QuoteDetailEditStrategyReviewComponent', () => {
  let component: QuoteDetailEditStrategyReviewComponent;
  let fixture: ComponentFixture<QuoteDetailEditStrategyReviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuoteDetailEditStrategyReviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuoteDetailEditStrategyReviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
