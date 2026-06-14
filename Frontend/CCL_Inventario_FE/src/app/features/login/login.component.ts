import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <div class="login-container">
      <div class="login-card">
        <h2>Control de Inventario</h2>
        <p class="subtitle">Inicie sesión para continuar</p>

        <form [formGroup]="loginForm" (ngSubmit)="onSubmit()">
          <div class="form-group">
            <label for="username">Usuario</label>
            <input 
              type="text" 
              id="username" 
              formControlName="username" 
              placeholder="Ingrese su usuario"
              [class.error]="loginForm.get('username')?.invalid && loginForm.get('username')?.touched"
            />
          </div>

          <div class="form-group">
            <label for="password">Contraseña</label>
            <input 
              type="password" 
              id="password" 
              formControlName="password" 
              placeholder="Ingrese su contraseña"
              [class.error]="loginForm.get('password')?.invalid && loginForm.get('password')?.touched"
            />
          </div>

          <div class="error-banner" *ngIf="errorMessage()">
            {{ errorMessage() }}
          </div>

          <button type="submit" [disabled]="loginForm.invalid || isLoading()">
            <span *ngIf="!isLoading()">Ingresar</span>
            <span *ngIf="isLoading()" class="spinner">Cargando...</span>
          </button>
        </form>
      </div>
    </div>
  `,
  styles: [`
    .login-container {
      display: flex;
      justify-content: center;
      align-items: center;
      min-height: 100vh;
      background-color: #f5f7fb;
      font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
    }
    .login-card {
      background: #ffffff;
      padding: 2.5rem;
      border-radius: 8px;
      box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
      width: 100%;
      max-width: 400px;
    }
    h2 {
      margin: 0 0 0.5rem 0;
      color: #1e293b;
      text-align: center;
    }
    .subtitle {
      margin: 0 0 2rem 0;
      color: #64748b;
      text-align: center;
      font-size: 0.9rem;
    }
    .form-group {
      margin-bottom: 1.25rem;
    }
    label {
      display: block;
      margin-bottom: 0.5rem;
      color: #475569;
      font-size: 0.85rem;
      font-weight: 600;
    }
    input {
      width: 100%;
      padding: 0.75rem;
      border: 1px solid #cbd5e1;
      border-radius: 6px;
      font-size: 0.95rem;
      box-sizing: border-box;
      transition: border-color 0.2s;
    }
    input:focus {
      outline: none;
      border-color: #3b82f6;
    }
    input.error {
      border-color: #ef4444;
      background-color: #fef2f2;
    }
    .error-banner {
      background-color: #fef2f2;
      color: #b91c1c;
      padding: 0.75rem;
      border-radius: 6px;
      font-size: 0.85rem;
      margin-bottom: 1.25rem;
      border: 1px solid #fee2e2;
      text-align: center;
    }
    button {
      width: 100%;
      padding: 0.75rem;
      background-color: #2563eb;
      color: white;
      border: none;
      border-radius: 6px;
      font-size: 1rem;
      font-weight: 600;
      cursor: pointer;
      transition: background-color 0.2s;
    }
    button:hover:not(:disabled) {
      background-color: #1d4ed8;
    }
    button:disabled {
      background-color: #94a3b8;
      cursor: not-allowed;
    }
    .spinner {
      display: inline-block;
      animation: pulse 1.5s infinite;
    }
    @keyframes pulse {
      0%, 100% { opacity: 1; }
      50% { opacity: 0.5; }
    }
  `]
})
export class LoginComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  // Estados usando Signals
  isLoading = signal<boolean>(false);
  errorMessage = signal<string | null>(null);

  loginForm: FormGroup = this.fb.group({
    username: ['', [Validators.required]],
    password: ['', [Validators.required]]
  });

  onSubmit(): void {
    if (this.loginForm.invalid) return;

    this.isLoading.set(true);
    this.errorMessage.set(null);

    this.authService.login(this.loginForm.value).subscribe({
      next: () => {
        this.router.navigate(['/inventario']);
      },
      error: (err: any) => {
        this.isLoading.set(false);
        if (err.status === 401) {
          this.errorMessage.set('Credenciales incorrectas. Intente de nuevo.');
        } else {
          this.errorMessage.set('Ocurrió un error en el servidor. Intente más tarde.');
        }
      }
    });
  }
}