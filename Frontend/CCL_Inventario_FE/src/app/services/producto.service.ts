import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface ProductoItem {
  id: number;
  nombre: string;
  cantidad: number;
}

interface MovimientoPayload {
  productoId: number;
  cantidad: number;
  tipoMovimiento: 'ENTRADA' | 'EGRESO';
}

@Injectable({
  providedIn: 'root'
})
export class ProductoService {
  private http = inject(HttpClient);

  private readonly apiUrl = 'https://localhost:7225/api/Productos';

  getInventario(): Observable<ProductoItem[]> {
    return this.http.get<ProductoItem[]>(this.apiUrl);
  }

  registrarMovimiento(payload: MovimientoPayload): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/movimiento`, payload);
  }
}