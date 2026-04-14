/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { QuoteAddSectionFromQuoteComponent } from './quote-add-section-from-quote.component';

describe('QuoteAddSectionFromQuoteComponent', () => {
  let component: QuoteAddSectionFromQuoteComponent;
  let fixture: ComponentFixture<QuoteAddSectionFromQuoteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuoteAddSectionFromQuoteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuoteAddSectionFromQuoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
