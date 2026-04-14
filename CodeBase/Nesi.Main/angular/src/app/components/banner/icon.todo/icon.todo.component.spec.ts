/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { IconTodoComponent } from './icon.todo.component';

describe('Icon.todoComponent', () => {
  let component: IconTodoComponent;
  let fixture: ComponentFixture<IconTodoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IconTodoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IconTodoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
