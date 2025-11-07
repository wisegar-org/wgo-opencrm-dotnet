import { Component } from '@angular/core';
import { AppModal } from "../app-modal/app-modal";

@Component({
  selector: 'app-dashboard',
  imports: [AppModal],
  templateUrl: './app-dashboard.html',
  styleUrl: './app-dashboard.css',
})
export class AppDashboard {}
