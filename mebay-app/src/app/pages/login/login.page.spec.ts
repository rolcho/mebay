import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginPage } from './login.page';

describe('LoginPage', () => {
  let component: LoginPage;
  let fixture: ComponentFixture<LoginPage>;

  beforeEach(async () => {
    fixture = TestBed.createComponent(LoginPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display title', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(
      compiled.querySelector('ion-card-header ion-card-title')?.textContent
    ).toContain('Login');
  });

  it('should have email field', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('ion-input#input-email')).toBeTruthy();
  });

  it('should have password field', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('ion-input#input-password')).toBeTruthy();
  });
});
