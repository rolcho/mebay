import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { ItemComponent } from './item.component';
import { HttpClientModule } from '@angular/common/http';

describe('ItemComponent', () => {
  let component: ItemComponent;
  let fixture: ComponentFixture<ItemComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [IonicModule.forRoot(), ItemComponent, HttpClientModule],
    }).compileComponents();

    fixture = TestBed.createComponent(ItemComponent);
    component = fixture.componentInstance;
    component.item = {
      id: 1,
      name: '',
      description: '',
      picture: '',
      price: 1,
      instantPrice: 1,
      sellerId: 1,
      seller: {
        name: '',
        email: '',
      },
      buyerId: 1,
      buyer: {
        name: '',
        email: '',
      },
    };
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
