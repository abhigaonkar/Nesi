/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { AdmintoolsService } from 'services/pages/admintools.service';

describe('Service: Admintools', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AdmintoolsService]
    });
  });

  it('should ...', inject([AdmintoolsService], (service: AdmintoolsService) => {
    expect(service).toBeTruthy();
  }));
});
