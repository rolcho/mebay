import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { IUserLoginRequest } from '../models/user-login-request.dto';
import { IUserLoginResponse } from '../models/user-login-response.dto';
import { IUserRegisterRequest } from '../models/user-register-request.dto.ts';
import { IUserRegisterResponse } from '../models/user-register-response.dto.ts';
import { HttpClient } from '@angular/common/http';
import { StorageService } from './storage.service';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  BASE_URL = `${environment.SERVER_URL}/user`;
  constructor(private http: HttpClient, private storage: StorageService) {}

  register(user: IUserRegisterRequest): Observable<IUserRegisterResponse> {
    return this.http.post<IUserRegisterResponse>(
      `${this.BASE_URL}/register`,
      user
    );
  }

  login(user: IUserLoginRequest): Observable<IUserLoginResponse> {
    return this.http.post<IUserLoginResponse>(`${this.BASE_URL}/login`, user);
  }

  logout(): void {
    this.storage.clear();
  }

  get name(): string {
    return this.storage.get('name');
  }

  get isAuthenticate(): boolean {
    return this.storage.get('token') !== null;
  }

  get isAdmin(): boolean {
    return this.storage.get('isAdmin') === true;
  }
}
