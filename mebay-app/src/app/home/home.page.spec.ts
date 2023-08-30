import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomePage } from './home.page';
import { IonicModule } from '@ionic/angular';

describe('HomePage', () => {
  let component: HomePage;
  let fixture: ComponentFixture<HomePage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [IonicModule],
    }).compileComponents();

    fixture = TestBed.createComponent(HomePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display First try', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(
      compiled.querySelector('ion-toolbar ion-title')?.textContent
    ).toContain('First try');
  });

  it('should not display Second try', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(
      compiled.querySelector('ion-toolbar ion-title')?.textContent
    ).not.toContain('Second try');
  });
});
