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
    this.storage.set('userId', parseInt(tokenContent.userId));
    this.storage.set('name', tokenContent.name);
    this.storage.set('tokenKey', token);
    this.storage.set('isAdmin', tokenContent.isAdmin);
  }

  isExpired(): boolean {
    const token = this.storage.get('tokenKey') as string;
    if (token === null || token === undefined) return true;
    return new JwtHelperService().isTokenExpired(token);
  }
}
