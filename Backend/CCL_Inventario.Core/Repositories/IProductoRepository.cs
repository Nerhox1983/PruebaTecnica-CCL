using CCL_Inventario.Core.Models;

namespace CCL_Inventario.Core.Repositories;

public interface IProductoRepository
{
    Task<IEnumerable<Producto>> ObtenerInventarioAsync();
    Task<Producto?> ObtenerPorIdAsync(int id);
    Task ActualizarCantidadAsync(Producto producto);
}
