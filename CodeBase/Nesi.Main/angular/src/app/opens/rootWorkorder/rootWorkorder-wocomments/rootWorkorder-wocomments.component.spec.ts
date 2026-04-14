/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { RootWorkorderWocommentsComponent } from './rootWorkorder-wocomments.component';

describe('RootWorkorderWocommentsComponent', () => {
  let component: RootWorkorderWocommentsComponent;
  let fixture: ComponentFixture<RootWorkorderWocommentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RootWorkorderWocommentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RootWorkorderWocommentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
