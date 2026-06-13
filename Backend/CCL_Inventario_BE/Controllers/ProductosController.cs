using CCL_Inventario.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CCL_Inventario.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController(IProductoRepository productoRepository) : ControllerBase
{
    /// <summary>
    /// GET: api/productos
    /// Obtiene la lista completa de productos para el inventario.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetInventario()
    {
        var productos = await productoRepository.ObtenerInventarioAsync();
        return Ok(productos);
    }
}