import { Routes } from '@angular/router';
import { LoginComponent } from './features/login/login.component';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'inventario',
    canActivate: [authGuard],
    loadComponent: () => import('./features/inventario-form/inventario-form.component').then(m => m.InventarioFormComponent)
  },
  {
    path: '',
    redirectTo: '/login',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: '/login'
  }
];