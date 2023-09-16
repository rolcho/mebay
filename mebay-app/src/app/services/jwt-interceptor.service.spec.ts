import { TestBed } from '@angular/core/testing';

import { JwtInterceptor } from './jwt-interceptor.service';
import { HttpClientModule } from '@angular/common/http';

describe('JwtInterceptor', () => {
  let service: JwtInterceptor;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientModule],
    });
    TestBed.configureTestingModule({});
    service = TestBed.inject(JwtInterceptor);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
