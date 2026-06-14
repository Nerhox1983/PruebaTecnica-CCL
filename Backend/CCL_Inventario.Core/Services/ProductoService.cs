using CCL_Inventario.Core.Interfaces;
using CCL_Inventario.Core.Models;
using CCL_Inventario.Core.Repositories;

namespace CCL_Inventario.Core.Services
{
    public class ProductoService(IProductoRepository productoRepository) : IProductoService
    {
        public async Task<IEnumerable<Producto>> ObtenerInventarioAsync()
        {
            return await productoRepository.ObtenerInventarioAsync();
        }

        public async Task<Producto> RegistrarMovimientoAsync(int productoId, int cantidad, string tipoMovimiento)
        {
            var producto = await productoRepository.ObtenerPorIdAsync(productoId);

            if (producto == null)
                throw new KeyNotFoundException($"Producto con ID {productoId} no encontrado.");

            string tipo = tipoMovimiento.ToUpper();

            if (tipo == "ENTRADA")
            {
                producto.Cantidad += cantidad;
            }
            else if (tipo == "EGRESO" || tipo == "SALIDA")
            {
                if (producto.Cantidad < cantidad)
                    throw new InvalidOperationException($"Stock insuficiente. Disponible: {producto.Cantidad} unidades.");

                producto.Cantidad -= cantidad;
            }
            else throw new ArgumentException("Tipo de movimiento inválido. Use 'ENTRADA' o 'EGRESO'.");

            await productoRepository.ActualizarCantidadAsync(producto);
            return producto;
        }
    }
}