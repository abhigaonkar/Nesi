/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { Timesheet_shop_projectsService } from './timesheet_shop_projects.service';

describe('Service: Timesheet_shop_projects', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [Timesheet_shop_projectsService]
    });
  });

  it('should ...', inject([Timesheet_shop_projectsService], (service: Timesheet_shop_projectsService) => {
    expect(service).toBeTruthy();
  }));
});
