import { Routes } from '@angular/router';
import { Login } from '../components/login/login';
import { Modal } from '../components/modal/modal';

export const routes: Routes = [
  { path: 'login', component: Login },
  { path: 'modal', component: Modal },
];
