import { Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full',
  },
  {
    path: 'tabs',
    loadComponent: () =>
      import('./pages/tabs/tabs.page').then((m) => m.TabsPage),
    canActivate: [AuthGuard],
    children: [
      {
        path: 'profile',
        loadComponent: () =>
          import('./pages/tabs/profile/profile.page').then(
            (m) => m.UserProfilePage
          ),
        canActivate: [AuthGuard],
      },
      {
        path: 'home',
        loadComponent: () =>
          import('./pages/tabs/home/home.page').then((m) => m.HomePage),
        canActivate: [AuthGuard],
      },
      {
        path: 'buy',
        loadComponent: () =>
          import('./pages/tabs/buy/buy.page').then((m) => m.BuyPage),
        canActivate: [AuthGuard],
      },
      {
        path: 'sell',
        loadComponent: () =>
          import('./pages/tabs/sell/sell.page').then((m) => m.SellPage),
        canActivate: [AuthGuard],
      },
    ],
  },
  {
    path: 'login',
    loadComponent: () =>
      import('./pages/login/login.page').then((m) => m.LoginPage),
  },
  {
    path: 'registration',
    loadComponent: () =>
      import('./pages/registration/registration.page').then(
        (m) => m.RegistrationPage
      ),
  },
];
