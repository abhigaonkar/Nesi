import { TestBed, inject } from '@angular/core/testing';

import { AddEditFormService } from './add-edit-form.service';

describe('AddEditFormService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [AddEditFormService]
    });
  });

  it('should be created', inject([AddEditFormService], (service: AddEditFormService) => {
    expect(service).toBeTruthy();
  }));
});
