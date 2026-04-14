/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { SwtichUserComponent } from './swtichUser.component';

describe('SwtichUserComponent', () => {
  let component: SwtichUserComponent;
  let fixture: ComponentFixture<SwtichUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SwtichUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SwtichUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
