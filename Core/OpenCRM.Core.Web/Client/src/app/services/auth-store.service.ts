import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { AuthUser } from '../models/auth/auth-user';

@Injectable({
  providedIn: 'root',
})
export class AuthStore {
  private readonly userSubject = new BehaviorSubject<AuthUser | null>(null);
  readonly user$ = this.userSubject.asObservable();

  setUser(user: AuthUser | null): void {
    this.userSubject.next(user);
  }

  clear(): void {
    this.userSubject.next(null);
  }

  get snapshot(): AuthUser | null {
    return this.userSubject.value;
  }
}
