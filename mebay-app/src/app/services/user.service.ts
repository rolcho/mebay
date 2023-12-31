import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { environment } from '../../environments/environment';
import { IUserLoginRequest } from '../models/user-login-request.dto';
import { IUserLoginResponse } from '../models/user-login-response.dto';
import { IUserRegisterRequest } from '../models/user-register-request.dto.ts';
import { IUserRegisterResponse } from '../models/user-register-response.dto.ts';
import { HttpClient } from '@angular/common/http';
import { StorageService } from './storage.service';
import { ITopUp } from '../models/user-topup.dto';
import { IUserResponse } from '../models/user-response.dto';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  BASE_URL = `${environment.SERVER_URL}/user`;
  constructor(private http: HttpClient, private storage: StorageService) {}

  register(user: IUserRegisterRequest): Observable<IUserRegisterResponse> {
    return this.http.post<IUserRegisterResponse>(
      `${this.BASE_URL}/register`,
      user
    );
  }

  update(
    user: IUserRegisterRequest,
    userId: number
  ): Observable<IUserRegisterResponse> {
    return this.http.put<IUserRegisterResponse>(
      `${this.BASE_URL}/${userId}`,
      user
    );
  }

  profile(userId: number): Observable<IUserRegisterResponse> {
    return this.http.get<IUserRegisterResponse>(`${this.BASE_URL}/${userId}`);
  }

  topUp(topUp: ITopUp): Observable<ITopUp> {
    return this.http.post<ITopUp>(`${this.BASE_URL}/credit`, topUp);
  }

  delete(userId: number): Observable<IUserRegisterResponse> {
    return this.http.delete<IUserRegisterResponse>(
      `${this.BASE_URL}/${userId}`
    );
  }

  login(user: IUserLoginRequest): Observable<IUserLoginResponse> {
    return this.http.post<IUserLoginResponse>(`${this.BASE_URL}/login`, user);
  }

  listUsers(): Observable<IUserResponse[]> {
    return this.http.get<IUserResponse[]>(`${this.BASE_URL}/list`);
  }

  getUser(): Observable<IUserResponse> {
    return this.http.get<IUserResponse>(`${this.BASE_URL}/${this.userId}`);
  }

  logout(): void {
    this.storage.clear();
  }

  get name(): string {
    return this.storage.get('name')!;
  }

  get userId(): number {
    return parseInt(this.storage.get('userId')!);
  }

  get isAuthenticated(): boolean {
    return this.storage.get('tokenKey') !== null;
  }

  get isAdmin(): boolean {
    return this.storage.get('isAdmin') === 'True';
  }

  get token(): string {
    return this.storage.get('tokenKey')!;
  }

  get credits(): number {
    return this.storage.get('credits')!;
  }

  set credits(amount: number) {
    this.storage.set('credits', amount)!;
  }
}
