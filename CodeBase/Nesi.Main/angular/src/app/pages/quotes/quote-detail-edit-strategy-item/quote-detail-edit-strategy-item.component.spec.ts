/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { QuoteDetailEditStrategyItemComponent } from './quote-detail-edit-strategy-item.component';

describe('QuoteDetailEditStrategyItemComponent', () => {
  let component: QuoteDetailEditStrategyItemComponent;
  let fixture: ComponentFixture<QuoteDetailEditStrategyItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuoteDetailEditStrategyItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuoteDetailEditStrategyItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
