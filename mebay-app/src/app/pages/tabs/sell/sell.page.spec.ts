import { ComponentFixture, TestBed } from '@angular/core/testing';
import { SellPage } from './sell.page';
import { HttpClientModule } from '@angular/common/http';

describe('SellPage', () => {
  let component: SellPage;
  let fixture: ComponentFixture<SellPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpClientModule],
    }).compileComponents();

    fixture = TestBed.createComponent(SellPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
