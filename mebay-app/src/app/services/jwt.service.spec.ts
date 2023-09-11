import { TestBed } from '@angular/core/testing';

import { JwtDecoderService } from './jwt.service';

describe('JwtService', () => {
  let service: JwtDecoderService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(JwtDecoderService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
