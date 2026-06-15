using CCL_Inventario.Api.Requests;
using CCL_Inventario.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CCL_Inventario.Api.Controllers;

/// <summary>
/// Controlador para la gestión y consulta de productos en el inventario.
/// </summary>
[ApiController]
[Route("productos")]
public class ProductosController(IProductoService productoService) : ControllerBase
{
    /// <summary>
    /// Obtiene la lista completa de productos para el inventario.
    /// </summary>
    /// <returns>Una lista de objetos de tipo Producto.</returns>
    /// <response code="200">Retorna el listado de productos.</response>
    [HttpGet("inventario")]
    public async Task<IActionResult> GetInventario()
    {
        var productos = await productoService.ObtenerInventarioAsync();
        return Ok(productos);
    }
    
    /// <summary>
    /// Registra un movimiento de inventario (ENTRADA o EGRESO) y actualiza el stock del producto.
    /// </summary>
    /// <param name="request">Datos del movimiento, incluyendo ID del producto, cantidad y tipo.</param>
    /// <returns>Información del producto con el stock actualizado tras el movimiento.</returns>
    /// <response code="200">Si el movimiento se procesó y persistió correctamente.</response>
    /// <response code="400">Si el tipo de movimiento es inválido o el stock es insuficiente para un egreso.</response>
    /// <response code="404">Si el producto solicitado no existe en la base de datos.</response>
    [HttpPost("movimiento")]
    public async Task<IActionResult> RegistrarMovimiento([FromBody] MovimientoRequest request)
    {
        try
        {
            var producto = await productoService.RegistrarMovimientoAsync(
                request.ProductoId, 
                request.Cantidad, 
                request.TipoMovimiento);

            return Ok(new
            {
                Message = "Movimiento registrado con éxito.",
                ProductoId = producto.Id,
                NombreProducto = producto.Nombre,
                NuevoStock = producto.Cantidad
            });
        }
        catch (KeyNotFoundException ex) { return NotFound(new { Message = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { Message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { Message = ex.Message }); }
        catch (Exception)
        {
            return StatusCode(500, new { Message = "Ocurrió un error inesperado al procesar el movimiento." });
        }
    }
}