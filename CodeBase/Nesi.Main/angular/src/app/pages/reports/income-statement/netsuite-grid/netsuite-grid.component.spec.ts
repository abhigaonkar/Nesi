import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NetsuiteGridComponent } from './netsuite-grid.component';

describe('NetsuiteGridComponent', () => {
  let component: NetsuiteGridComponent;
  let fixture: ComponentFixture<NetsuiteGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NetsuiteGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NetsuiteGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
