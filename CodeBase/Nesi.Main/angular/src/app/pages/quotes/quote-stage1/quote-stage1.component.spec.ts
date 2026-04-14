/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { QuoteStage1Component } from './quote-stage1.component';

describe('QuoteStage1Component', () => {
  let component: QuoteStage1Component;
  let fixture: ComponentFixture<QuoteStage1Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuoteStage1Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuoteStage1Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
