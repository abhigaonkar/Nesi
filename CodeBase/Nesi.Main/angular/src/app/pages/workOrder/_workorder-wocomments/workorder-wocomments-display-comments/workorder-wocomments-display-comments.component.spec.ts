/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { WorkorderWocommentsDisplayCommentsComponent } from './workorder-wocomments-display-comments.component';

describe('WorkorderWocommentsDisplayCommentsComponent', () => {
  let component: WorkorderWocommentsDisplayCommentsComponent;
  let fixture: ComponentFixture<WorkorderWocommentsDisplayCommentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WorkorderWocommentsDisplayCommentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkorderWocommentsDisplayCommentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
