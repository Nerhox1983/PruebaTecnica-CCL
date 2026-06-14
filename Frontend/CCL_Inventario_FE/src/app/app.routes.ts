import { Routes } from '@angular/router';
import { LoginComponent } from './features/login/login.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: 'inventario',
    loadComponent: () => import('./features/inventario-lista/inventario-lista.component').then(m => m.InventarioListaComponent)
  },
  {
    path: 'movimiento',
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