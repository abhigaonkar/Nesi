/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { MessagespageInboxComponent } from './messagespage-inbox.component';

describe('MessagespageInboxComponent', () => {
  let component: MessagespageInboxComponent;
  let fixture: ComponentFixture<MessagespageInboxComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MessagespageInboxComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MessagespageInboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
