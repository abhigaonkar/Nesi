/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { QuoteAddSectionFromSpecificComponent } from './quote-add-section-from-specific.component';

describe('QuoteAddSectionFromSpecificComponent', () => {
  let component: QuoteAddSectionFromSpecificComponent;
  let fixture: ComponentFixture<QuoteAddSectionFromSpecificComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ QuoteAddSectionFromSpecificComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(QuoteAddSectionFromSpecificComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
