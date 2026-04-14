import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvancepayreportComponent } from './advancepayreport.component';

describe('AdvancepayreportComponent', () => {
  let component: AdvancepayreportComponent;
  let fixture: ComponentFixture<AdvancepayreportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdvancepayreportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdvancepayreportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
