/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { ReadService } from './read.service';

describe('Service: Read', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ReadService]
    });
  });

  it('should ...', inject([ReadService], (service: ReadService) => {
    expect(service).toBeTruthy();
  }));
});
