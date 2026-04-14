/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { IconFvrComponent } from './icon.fvr.component';

describe('Icon.fvrComponent', () => {
  let component: IconFvrComponent;
  let fixture: ComponentFixture<IconFvrComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IconFvrComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IconFvrComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
