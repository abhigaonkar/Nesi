/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';
import { WhoDoIAskTableComponent } from './whoDoIAskTable.component';

describe('WhoDoIAskTableComponent', () => {
  let component: WhoDoIAskTableComponent;
  let fixture: ComponentFixture<WhoDoIAskTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WhoDoIAskTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WhoDoIAskTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
