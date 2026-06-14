using CCL_Inventario.Core.Models;

namespace CCL_Inventario.Core.Interfaces;

public interface IProductoService
{
    Task<IEnumerable<Producto>> ObtenerInventarioAsync();
    Task<Producto> RegistrarMovimientoAsync(int productoId, int cantidad, string tipoMovimiento);
}