import { Component, inject } from '@angular/core';
import { ChartPie } from '../../icons/chart-pie/chart-pie';
import { ApiService } from '../../services/api-service';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  imports: [ChartPie, FormsModule],
  templateUrl: './app-login.html',
  styleUrl: './app-login.css',
})
export class AppLogin {
  usernameInput: string = '';
  passwordInput: string = '';
  persistenceInput: boolean = true;

  private router = inject(Router);

  constructor(private apiService: ApiService) {}

  onLogin() {
    this.apiService
      .sendPost('/identity/signin', { username: this.usernameInput, password: this.passwordInput })
      .subscribe({
        next: (res) => {
          console.log('Respuesta:', res);
          this.router.navigate(['/']);
        },
        error: (err) => {
          console.error('Error:', err);
        },
      });
  }
}
