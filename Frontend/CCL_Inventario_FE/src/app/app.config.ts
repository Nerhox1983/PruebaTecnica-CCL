import { ApplicationConfig, provideZonelessChangeDetection } from '@angular/core'; // <-- 1. QUITA EL 'Experimental' AQUÍ
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http'; 

import { routes } from './app.routes';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZonelessChangeDetection(), // <-- 2. QUITA EL 'Experimental' AQUÍ TAMBIÉN
    provideRouter(routes),
    provideHttpClient() 
  ]
};