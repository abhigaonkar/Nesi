import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MasterResponsiblilitesGridComponent } from './master-responsiblilites-grid.component';

describe('MasterResponsiblilitesGridComponent', () => {
  let component: MasterResponsiblilitesGridComponent;
  let fixture: ComponentFixture<MasterResponsiblilitesGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MasterResponsiblilitesGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MasterResponsiblilitesGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
