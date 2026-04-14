/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { VendorContactsBvItemComponent } from './vendor-contacts-bv-item.component';

describe('VendorContactsBvItemComponent', () => {
  let component: VendorContactsBvItemComponent;
  let fixture: ComponentFixture<VendorContactsBvItemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VendorContactsBvItemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VendorContactsBvItemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
