/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RootquotesDetail2Component } from './rootquotesDetail2.component';

describe('RootquotesDetail2Component', () => {
  let component: RootquotesDetail2Component;
  let fixture: ComponentFixture<RootquotesDetail2Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RootquotesDetail2Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RootquotesDetail2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
