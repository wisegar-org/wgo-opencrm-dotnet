import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AppHeader } from '../app-header/app-header';
import { AppDrawer } from '../app-drawer/app-drawer';
import { AppFooter } from '../app-footer/app-footer';

@Component({
  selector: 'app-layout',
  imports: [RouterOutlet, AppHeader, AppDrawer, AppFooter],
  templateUrl: './app-layout.html',
  styleUrl: './app-layout.css',
})
export class AppLayout {}
