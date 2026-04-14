/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { MessagespageReplyComponent } from './messagespage-reply.component';

describe('MessagespageReplyComponent', () => {
  let component: MessagespageReplyComponent;
  let fixture: ComponentFixture<MessagespageReplyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MessagespageReplyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MessagespageReplyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
