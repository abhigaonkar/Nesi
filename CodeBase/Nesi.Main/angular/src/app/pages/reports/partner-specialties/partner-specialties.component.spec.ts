import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PartnerSpecialtiesComponent } from './partner-specialties.component';

describe('PartnerSpecialtiesComponent', () => {
  let component: PartnerSpecialtiesComponent;
  let fixture: ComponentFixture<PartnerSpecialtiesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PartnerSpecialtiesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PartnerSpecialtiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
