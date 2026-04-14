/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { IconOnlineUserComponent } from './icon.onlineUser.component';

describe('Icon.onlineUserComponent', () => {
  let component: IconOnlineUserComponent;
  let fixture: ComponentFixture<IconOnlineUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IconOnlineUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IconOnlineUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
