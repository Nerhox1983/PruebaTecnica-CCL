import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // Cambiado a FormsModule para soportar [(ngModel)] del HTML
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth';
import { ProductoService, ProductoItem } from '../../services/producto.service';

@Component({
  selector: 'app-inventario-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './inventario-form.component.html',
  styleUrl: './inventario-form.component.css'
})
export class InventarioFormComponent implements OnInit {
  private authService = inject(AuthService);
  private productoService = inject(ProductoService);
  private router = inject(Router);

  productos = signal<ProductoItem[]>([]);
  
  mensajeExito: string | null = null;
  mensajeError: string | null = null;

  mostrarModal: boolean = false;
  productoSeleccionado: ProductoItem | null = null;
  tipoMovimiento: 'ENTRADA' | 'EGRESO' = 'ENTRADA';
  cantidadMovimiento: number = 1;

  ngOnInit(): void {
    this.cargarInventario();
  }

  cargarInventario(): void {
    this.productoService.getInventario().subscribe({
      next: (data) => {
        this.productos.set(data);
      },
      error: (err) => {
        this.mostrarAlerta('error', 'No se pudo cargar el inventario. Verifique la conexión.');
        console.error(err);
      }
    });
  }

  onLogout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  abrirModalMovimiento(producto: ProductoItem): void {
    this.productoSeleccionado = producto;
    this.tipoMovimiento = 'ENTRADA';
    this.cantidadMovimiento = 1;
    this.mostrarModal = true;
  }

  cerrarModal(): void {
    this.mostrarModal = false;
    this.productoSeleccionado = null;
  }

  guardarMovimiento(): void {
    if (!this.productoSeleccionado) return;
    
    if (this.cantidadMovimiento <= 0) {
      this.mostrarAlerta('error', 'La cantidad del movimiento debe ser mayor a cero.');
      return;
    }

    if (this.tipoMovimiento === 'EGRESO' && this.cantidadMovimiento > this.productoSeleccionado.cantidad) {
      this.mostrarAlerta('error', `Stock insuficiente. Solo tienes ${this.productoSeleccionado.cantidad} unidades disponibles.`);
      return;
    }

    const payload = {
      productoId: this.productoSeleccionado.id,
      cantidad: this.cantidadMovimiento,
      tipoMovimiento: this.tipoMovimiento
    };

    this.productoService.registrarMovimiento(payload).subscribe({
      next: () => {
        this.mostrarAlerta('exito', `Movimiento de ${this.tipoMovimiento} procesado con éxito.`);
        this.cerrarModal();
        this.cargarInventario();
      },
      error: (err) => {
        this.mostrarAlerta('error', 'Error al procesar el movimiento en el servidor.');
        console.error(err);
      }
    });
  }

  private mostrarAlerta(tipo: 'exito' | 'error', mensaje: string): void {
    if (tipo === 'exito') {
      this.mensajeExito = mensaje;
      setTimeout(() => this.mensajeExito = null, 4000);
    } else {
      this.mensajeError = mensaje;
      setTimeout(() => this.mensajeError = null, 4000);
    }
  }
}