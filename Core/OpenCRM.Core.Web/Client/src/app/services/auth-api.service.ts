import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { LoginRequest } from '../models/auth/login-request';
import { LoginResponse } from '../models/auth/login-response';
import { RegisterRequest } from '../models/auth/register-request';
import { RegisterResponse } from '../models/auth/register-response';
import { ConfirmEmailRequest } from '../models/auth/confirm-email-request';
import { ConfirmEmailResponse } from '../models/auth/confirm-email-response';

@Injectable({
  providedIn: 'root',
})
export class AuthApiService {
  constructor(private readonly api: ApiService) {}

  login(body: LoginRequest): Observable<LoginResponse> {
    return this.api.post<LoginRequest, LoginResponse>('auth/login', body);
  }

  register(body: RegisterRequest): Observable<RegisterResponse> {
    return this.api.post<RegisterRequest, RegisterResponse>('auth/register', body);
  }

  confirmEmail(query: ConfirmEmailRequest): Observable<ConfirmEmailResponse> {
    const params = { userId: query.userId, token: query.token };
    return this.api.get<ConfirmEmailResponse>('auth/confirm-email', params);
  }
}
