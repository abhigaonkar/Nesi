/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { Nesi1Component } from './Nesi1.component';

describe('Nesi1Component', () => {
  let component: Nesi1Component;
  let fixture: ComponentFixture<Nesi1Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ Nesi1Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(Nesi1Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
