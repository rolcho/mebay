import { ComponentFixture, TestBed } from '@angular/core/testing';
import { TabsPage } from './tabs.page';
import { BehaviorSubject } from 'rxjs';
import { ActivatedRoute, ParamMap, convertToParamMap } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

class ActivatedRouteMock {
  private paramMapSubject = new BehaviorSubject(convertToParamMap({}));
  paramMap = this.paramMapSubject.asObservable();

  setParamMap(params: ParamMap) {
    this.paramMapSubject.next(convertToParamMap(params));
  }
}
describe('TabsPage', () => {
  let component: TabsPage;
  let fixture: ComponentFixture<TabsPage>;

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

    fixture = TestBed.createComponent(TabsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
