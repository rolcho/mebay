import { ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';
import { TabsComponent } from './tabs.component';
import { ActivatedRoute } from '@angular/router';
import { convertToParamMap, ParamMap } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { UserService } from '../../services/user.service';
import { HttpClientModule } from '@angular/common/http';

class ActivatedRouteMock {
  private paramMapSubject = new BehaviorSubject(convertToParamMap({}));
  paramMap = this.paramMapSubject.asObservable();

  setParamMap(params: ParamMap) {
    this.paramMapSubject.next(convertToParamMap(params));
  }
}

describe('TabsComponent', () => {
  let component: TabsComponent;
  let fixture: ComponentFixture<TabsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IonicModule.forRoot(), HttpClientModule],
      providers: [
        UserService,
        {
          provide: ActivatedRoute,
          useClass: ActivatedRouteMock, // Provide the mock here
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(TabsComponent);
    component = fixture.componentInstance;
  });

  it('should create', () => {
    fixture.detectChanges();
    expect(component).toBeTruthy();
  });
});
