import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root',
})
export class JwtDecoderService {
  constructor(private storage: StorageService) {}

  decode(token: string) {
    const tokenContent = new JwtHelperService().decodeToken(token);
    const isAdmin =
      tokenContent[
        'http://schemas.microsoft.com/ws/2008/06/identity/claims/role'
      ] === 'Admin';
    this.storage.set('userId', tokenContent.userId);
    this.storage.set('name', tokenContent.name);
    this.storage.set('token', token);
    this.storage.set('isAdmin', isAdmin);
  }
}
