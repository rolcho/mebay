import { ComponentFixture, TestBed } from '@angular/core/testing';
import { UserProfilePage } from './profile.page';
import { HttpClientModule } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';
import { convertToParamMap, ParamMap } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

class ActivatedRouteMock {
  private paramMapSubject = new BehaviorSubject(convertToParamMap({}));
  paramMap = this.paramMapSubject.asObservable();

  setParamMap(params: ParamMap) {
    this.paramMapSubject.next(convertToParamMap(params));
  }
}

describe('UserProfilePage', () => {
  let component: UserProfilePage;
  let fixture: ComponentFixture<UserProfilePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientModule],
      providers: [
        {
          provide: ActivatedRoute,
          useClass: ActivatedRouteMock, // Provide the mock here
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(UserProfilePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
