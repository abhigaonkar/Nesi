/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { CustomerRequestAdminNewComponent } from './customerrequestadminNew.component';

describe('CustomerRequestAdminNewComponent', () => {
  let component: CustomerRequestAdminNewComponent;
  let fixture: ComponentFixture<CustomerRequestAdminNewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CustomerRequestAdminNewComponent]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomerRequestAdminNewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
