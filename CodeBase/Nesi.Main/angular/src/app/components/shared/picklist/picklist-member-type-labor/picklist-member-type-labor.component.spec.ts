/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { PicklistMemberTypeLaborComponent } from './picklist-member-type-labor.component';

describe('PicklistMemberTypeLaborComponent', () => {
  let component: PicklistMemberTypeLaborComponent;
  let fixture: ComponentFixture<PicklistMemberTypeLaborComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PicklistMemberTypeLaborComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PicklistMemberTypeLaborComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
