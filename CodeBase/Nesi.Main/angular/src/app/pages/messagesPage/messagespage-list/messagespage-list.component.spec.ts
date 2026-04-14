/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { MessagespageListComponent } from './messagespage-list.component';

describe('MessagespageListComponent', () => {
  let component: MessagespageListComponent;
  let fixture: ComponentFixture<MessagespageListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MessagespageListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MessagespageListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
