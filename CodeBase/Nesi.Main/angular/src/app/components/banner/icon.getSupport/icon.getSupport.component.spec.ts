/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { IconGetSupportComponent } from './icon.getSupport.component';

describe('Icon.getSupportComponent', () => {
  let component: IconGetSupportComponent;
  let fixture: ComponentFixture<IconGetSupportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IconGetSupportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IconGetSupportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
