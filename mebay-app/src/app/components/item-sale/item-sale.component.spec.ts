import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { ItemSaleComponent } from './item-sale.component';
import { HttpClientModule } from '@angular/common/http';

describe('ItemSaleComponent', () => {
  let component: ItemSaleComponent;
  let fixture: ComponentFixture<ItemSaleComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      imports: [IonicModule.forRoot(), ItemSaleComponent, HttpClientModule],
    }).compileComponents();

    fixture = TestBed.createComponent(ItemSaleComponent);
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
