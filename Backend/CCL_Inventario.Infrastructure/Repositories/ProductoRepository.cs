using CCL_Inventario.Core.Models;
using CCL_Inventario.Core.Repositories;
using CCL_Inventario.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CCL_Inventario.Infrastructure.Repositories
{
    public class ProductoRepository(InventarioDbContext context) : IProductoRepository
    {
        public async Task<IEnumerable<Producto>> ObtenerInventarioAsync()
        {
            return await context.Productos.AsNoTracking().ToListAsync();
        }

        public async Task<Producto?> ObtenerPorIdAsync(int id)
        {
            return await context.Productos.FindAsync(id);
        }

        public async Task ActualizarCantidadAsync(Producto producto)
        {
            context.Productos.Update(producto);
            await context.SaveChangesAsync();
        }
    }
}