import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ParameterComponentComponent } from './parameter-component.component';

describe('ParameterComponentComponent', () => {
  let component: ParameterComponentComponent;
  let fixture: ComponentFixture<ParameterComponentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ParameterComponentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ParameterComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
