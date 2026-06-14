using Moq;
using CCL_Inventario.Core.Services;
using CCL_Inventario.Core.Repositories;
using CCL_Inventario.Core.Models;

namespace CCL_Inventario.Tests
{
    /// <summary>
    /// Pruebas unitarias para las reglas de negocio de productos y stock.
    /// </summary>
    public class ProductoServiceTests
    {
        private readonly Mock<IProductoRepository> _productoRepositoryMock;
        private readonly ProductoService _productoService;

        public ProductoServiceTests()
        {
            _productoRepositoryMock = new Mock<IProductoRepository>();
            _productoService = new ProductoService(_productoRepositoryMock.Object);
        }

        /// <summary>
        /// Valida que se recupere la lista de productos delegando la llamada al repositorio.
        /// </summary>
        [Fact]
        public async Task ObtenerInventarioAsync_DeberiaRetornarListaDeProductos()
        {
            var productosFalsos = new List<Producto>
            {
                new Producto { Id = 1, Nombre = "Producto A", Cantidad = 10 }
            };

            _productoRepositoryMock
                .Setup(r => r.ObtenerInventarioAsync())
                .ReturnsAsync(productosFalsos);

            var resultado = await _productoService.ObtenerInventarioAsync();

            Assert.NotNull(resultado);
            _productoRepositoryMock.Verify(r => r.ObtenerInventarioAsync(), Times.Once);
        }

        /// <summary>
        /// Valida que el servicio lance una excepción si el ID de producto no existe al registrar movimiento.
        /// </summary>
        [Fact]
        public async Task RegistrarMovimientoAsync_DeberiaLanzarKeyNotFoundException_CuandoElProductoNoExiste()
        {
            int productoIdInexistente = 99;
            _productoRepositoryMock
                .Setup(r => r.ObtenerPorIdAsync(productoIdInexistente))
                .ReturnsAsync((Producto)null!);

            var ex = await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
                await _productoService.RegistrarMovimientoAsync(productoIdInexistente, 5, "ENTRADA")
            );
            Assert.Contains("no encontrado", ex.Message);
        }

        /// <summary>
        /// Verifica que el stock se incremente correctamente para movimientos de tipo ENTRADA.
        /// </summary>
        [Fact]
        public async Task RegistrarMovimientoAsync_DeberiaIncrementarStock_CuandoElTipoEsEntrada()
        {
            var producto = new Producto { Id = 1, Nombre = "Producto Test", Cantidad = 10 };
            _productoRepositoryMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(producto);
            _productoRepositoryMock.Setup(r => r.ActualizarCantidadAsync(producto)).Returns(Task.CompletedTask);

            var resultado = await _productoService.RegistrarMovimientoAsync(1, 5, "entrada");

            Assert.Equal(15, resultado.Cantidad);
            _productoRepositoryMock.Verify(r => r.ActualizarCantidadAsync(It.Is<Producto>(p => p.Cantidad == 15)), Times.Once);
        }

        /// <summary>
        /// Verifica que el stock se reduzca correctamente para movimientos de tipo EGRESO.
        /// </summary>
        [Fact]
        public async Task RegistrarMovimientoAsync_DeberiaDecrementarStock_CuandoElTipoEsEgresoYHayStock()
        {
            var producto = new Producto { Id = 1, Nombre = "Producto Test", Cantidad = 10 };
            _productoRepositoryMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(producto);
            _productoRepositoryMock.Setup(r => r.ActualizarCantidadAsync(producto)).Returns(Task.CompletedTask);

            var resultado = await _productoService.RegistrarMovimientoAsync(1, 4, "EGRESO");

            Assert.Equal(6, resultado.Cantidad);
            _productoRepositoryMock.Verify(r => r.ActualizarCantidadAsync(It.Is<Producto>(p => p.Cantidad == 6)), Times.Once);
        }

        /// <summary>
        /// Verifica que el tipo de movimiento SALIDA también sea procesado como un egreso válido.
        /// </summary>
        [Fact]
        public async Task RegistrarMovimientoAsync_DeberiaDecrementarStock_CuandoElTipoEsSalidaYHayStock()
        {
            var producto = new Producto { Id = 1, Nombre = "Producto Test", Cantidad = 10 };
            _productoRepositoryMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(producto);
            _productoRepositoryMock.Setup(r => r.ActualizarCantidadAsync(producto)).Returns(Task.CompletedTask);

            var resultado = await _productoService.RegistrarMovimientoAsync(1, 3, "SALIDA");

            Assert.Equal(7, resultado.Cantidad);
            _productoRepositoryMock.Verify(r => r.ActualizarCantidadAsync(It.Is<Producto>(p => p.Cantidad == 7)), Times.Once);
        }

        /// <summary>
        /// Valida que se lance una excepción si se intenta retirar más stock del disponible.
        /// </summary>
        [Fact]
        public async Task RegistrarMovimientoAsync_DeberiaLanzarInvalidOperationException_CuandoStockEsInsuficiente()
        {
            var producto = new Producto { Id = 1, Nombre = "Producto Test", Cantidad = 2 };
            _productoRepositoryMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(producto);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await _productoService.RegistrarMovimientoAsync(1, 5, "EGRESO")
            );
            Assert.Contains("Stock insuficiente", ex.Message);
        }

        /// <summary>
        /// Valida que se lance una excepción de argumento si el tipo de movimiento no es ENTRADA, EGRESO o SALIDA.
        /// </summary>
        [Fact]
        public async Task RegistrarMovimientoAsync_DeberiaLanzarArgumentException_CuandoElTipoEsInvalido()
        {
            var producto = new Producto { Id = 1, Nombre = "Producto Test", Cantidad = 10 };
            _productoRepositoryMock.Setup(r => r.ObtenerPorIdAsync(1)).ReturnsAsync(producto);

            var ex = await Assert.ThrowsAsync<ArgumentException>(async () =>
                await _productoService.RegistrarMovimientoAsync(1, 5, "INVALIDO")
            );
            Assert.Contains("Tipo de movimiento inválido", ex.Message);
        }
    }
}