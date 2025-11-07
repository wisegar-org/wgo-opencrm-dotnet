import { Routes } from '@angular/router';
import { AppLogin } from '../components/app-login/app-login';
import { AppLayout } from '../components/app-layout/app-layout';
import { AppDashboard } from '../components/app-dashboard/app-dashboard';
import { App404 } from '../components/app-404/app-404';

export const routes: Routes = [
  { path: 'login', component: AppLogin },
  {
    path: '',
    component: AppLayout,
    children: [
      {
        path: '',
        component: AppDashboard,
      },
    ],
  },
  {
    path: '**',
    pathMatch: 'full',
    component: App404,
  },
];
