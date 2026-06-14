import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  // Revisa que este puerto coincida con el que corre tu API de .NET
  private readonly apiUrl = 'https://localhost:7117/api/auth'; 

  currentUserToken = signal<string | null>(localStorage.getItem('token'));

  login(credentials: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, credentials).pipe(
      tap(response => {
        if (response && response.token) {
          localStorage.setItem('token', response.token);
          this.currentUserToken.set(response.token);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem('token');
    this.currentUserToken.set(null);
  }

  isAuthenticated(): boolean {
    return !!this.currentUserToken();
  }
}