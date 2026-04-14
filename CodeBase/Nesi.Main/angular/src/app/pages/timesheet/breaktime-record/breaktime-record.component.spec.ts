import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BreaktimeRecordComponent } from './breaktime-record.component';

describe('BreaktimeRecordComponent', () => {
  let component: BreaktimeRecordComponent;
  let fixture: ComponentFixture<BreaktimeRecordComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BreaktimeRecordComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BreaktimeRecordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
