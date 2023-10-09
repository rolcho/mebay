import { ComponentFixture, TestBed } from '@angular/core/testing';
import { BuyPage } from './buy.page';
import { HttpClientModule } from '@angular/common/http';

describe('BuyPage', () => {
  let component: BuyPage;
  let fixture: ComponentFixture<BuyPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientModule],
    }).compileComponents();

    fixture = TestBed.createComponent(BuyPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
