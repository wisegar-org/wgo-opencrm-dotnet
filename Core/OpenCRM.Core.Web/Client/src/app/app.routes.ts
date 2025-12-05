import { Routes } from '@angular/router';
import { LoginPage } from './login/login.page';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  { path: 'login', component: LoginPage },
];
