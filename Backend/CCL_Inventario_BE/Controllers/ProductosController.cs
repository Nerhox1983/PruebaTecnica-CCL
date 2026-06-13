using CCL_Inventario.Core.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CCL_Inventario.Api.Controllers;

/// <summary>
/// Controlador para la gestión y consulta de productos en el inventario.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductosController(IProductoRepository productoRepository) : ControllerBase
{
    /// <summary>
    /// Obtiene la lista completa de productos para el inventario.
    /// </summary>
    /// <returns>Una lista de objetos de tipo Producto.</returns>
    /// <response code="200">Retorna el listado de productos.</response>
    [HttpGet]
    public async Task<IActionResult> GetInventario()
    {
        var productos = await productoRepository.ObtenerInventarioAsync();
        return Ok(productos);
    }
}