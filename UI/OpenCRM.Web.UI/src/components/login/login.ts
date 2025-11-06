import { Component } from '@angular/core';
import { ChartPie } from '../../icons/chart-pie/chart-pie';
import { ApiService } from '../../services/api-service';

@Component({
  selector: 'app-login',
  imports: [ChartPie],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  constructor(private apiService: ApiService) {}
  usernameInput: string = '';
  passwordInput: string = '';

  onLogin() {
    this.apiService
      .sendPost('/identity/signin', { username: this.usernameInput, password: this.passwordInput })
      .subscribe({
        next: (res) => console.log('Respuesta:', res),
        error: (err) => console.error('Error:', err),
      });
  }
}
