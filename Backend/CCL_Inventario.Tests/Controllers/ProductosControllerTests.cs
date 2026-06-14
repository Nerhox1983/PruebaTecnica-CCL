using CCL_Inventario.Api.Controllers;
using CCL_Inventario.Api.Requests; // Ajusta según tus namespaces
using CCL_Inventario.Core.Interfaces;
using CCL_Inventario.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;

/// <summary>
/// Pruebas unitarias para el controlador de productos y movimientos de inventario.
/// </summary>
public class ProductosControllerTests
{
    private readonly Mock<IProductoService> _productoServiceMock;
    private readonly ProductosController _controller;

    public ProductosControllerTests()
    {
        _productoServiceMock = new Mock<IProductoService>();
        _controller = new ProductosController(_productoServiceMock.Object);
    }

    /// <summary>
    /// Verifica que el registro de un movimiento retorne 200 (OK) en condiciones normales.
    /// </summary>
    [Fact]
    public async Task RegistrarMovimiento_DeberiaRetornarOk_CuandoElMovimientoEsExitoso()
    {
        var request = new MovimientoRequest { ProductoId = 1, Cantidad = 5, TipoMovimiento = "ENTRADA" };
        var productoRetornado = new Producto { Id = 1, Nombre = "Producto Test", Cantidad = 10 };

        _productoServiceMock
            .Setup(s => s.RegistrarMovimientoAsync(request.ProductoId, request.Cantidad, request.TipoMovimiento))
            .ReturnsAsync(productoRetornado);

        var result = await _controller.RegistrarMovimiento(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    /// <summary>
    /// Verifica que se retorne 404 si se intenta registrar un movimiento para un producto inexistente.
    /// </summary>
    [Fact]
    public async Task RegistrarMovimiento_DeberiaRetornarNotFound_CuandoElProductoNoExiste()
    {
        var request = new MovimientoRequest { ProductoId = 99, Cantidad = 1, TipoMovimiento = "ENTRADA" };
        var mensajeError = "El producto solicitado no existe.";

        _productoServiceMock
            .Setup(s => s.RegistrarMovimientoAsync(request.ProductoId, request.Cantidad, request.TipoMovimiento))
            .ThrowsAsync(new KeyNotFoundException(mensajeError));

        var result = await _controller.RegistrarMovimiento(request);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.Equal(404, notFoundResult.StatusCode);
    }

    /// <summary>
    /// Verifica que se retorne 400 (Bad Request) ante una operación inválida como stock insuficiente.
    /// </summary>
    [Fact]
    public async Task RegistrarMovimiento_DeberiaRetornarBadRequest_CuandoLaOperacionEsInvalida()
    {
        var request = new MovimientoRequest { ProductoId = 1, Cantidad = 100, TipoMovimiento = "EGRESO" };
        var mensajeError = "Stock insuficiente para realizar el egreso.";

        _productoServiceMock
            .Setup(s => s.RegistrarMovimientoAsync(request.ProductoId, request.Cantidad, request.TipoMovimiento))
            .ThrowsAsync(new InvalidOperationException(mensajeError));

        var result = await _controller.RegistrarMovimiento(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    /// <summary>
    /// Verifica que se retorne 400 si el tipo de movimiento no es reconocido.
    /// </summary>
    [Fact]
    public async Task RegistrarMovimiento_DeberiaRetornarBadRequest_CuandoElArgumentoEsInvalido()
    {
        var request = new MovimientoRequest { ProductoId = 1, Cantidad = 5, TipoMovimiento = "INVALIDO" };
        var mensajeError = "El tipo de movimiento es inválido.";

        _productoServiceMock
            .Setup(s => s.RegistrarMovimientoAsync(request.ProductoId, request.Cantidad, request.TipoMovimiento))
            .ThrowsAsync(new ArgumentException(mensajeError));

        var result = await _controller.RegistrarMovimiento(request);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.Equal(400, badRequestResult.StatusCode);
    }

    /// <summary>
    /// Verifica que cualquier excepción no controlada resulte en un código 500.
    /// </summary>
    [Fact]
    public async Task RegistrarMovimiento_DeberiaRetornarInternalServerError_CuandoOcurreUnErrorInesperado()
    {
        var request = new MovimientoRequest { ProductoId = 1, Cantidad = 5, TipoMovimiento = "ENTRADA" };

        _productoServiceMock
            .Setup(s => s.RegistrarMovimientoAsync(request.ProductoId, request.Cantidad, request.TipoMovimiento))
            .ThrowsAsync(new System.Exception("Error de base de datos o caída del servidor"));

        var result = await _controller.RegistrarMovimiento(request);       

        var statusCodeResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, statusCodeResult.StatusCode);
    }
    
    /// <summary>
    /// Valida que la obtención del inventario retorne el listado completo de productos.
    /// </summary>
    [Fact]
    public async Task GetInventario_DeberiaRetornarOkConListaDeProductos_CuandoExistanProductos()
    {
        var productosFalsos = new List<Producto>
        {
            new Producto { Id = 1, Nombre = "Producto A", Cantidad = 10 },
            new Producto { Id = 2, Nombre = "Producto B", Cantidad = 5 }
        };

        _productoServiceMock
            .Setup(s => s.ObtenerInventarioAsync())
            .ReturnsAsync(productosFalsos);

        var result = await _controller.GetInventario();

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        var listaRetornada = Assert.IsType<List<Producto>>(okResult.Value);
        Assert.Equal(2, listaRetornada.Count);
    }
}