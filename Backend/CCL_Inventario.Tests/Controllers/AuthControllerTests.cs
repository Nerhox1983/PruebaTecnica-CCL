using CCL_Inventario.Api.Controllers;
using CCL_Inventario.Api.Requests;
using CCL_Inventario.Core.DTOs;
using CCL_Inventario.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CCL_Inventario.Tests.Controllers
{
    /// <summary>
    /// Pruebas unitarias para el controlador de autenticación.
    /// </summary>
    public class AuthControllerTests
    {
        private readonly Mock<IAuthService> _authServiceMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authServiceMock = new Mock<IAuthService>();
            _controller = new AuthController( _authServiceMock.Object ); 
        }

        /// <summary>
        /// Valida que el endpoint de login retorne un código 200 (OK) al recibir credenciales válidas.
        /// </summary>
        [Fact]
        public async Task Login_DeberiaRetornarOkConToken_CuandoLasCredencialesSonValidas()
        {
            var request = new LoginRequest
            {
                Username = "admin",
                Password = "admin123"
            };

            var respuestaEsperada = new AuthResponseDto { Token = "jwt.token.simulado" };

            _authServiceMock
                .Setup(s => s.LoginAsync(request.Username, request.Password))
                .ReturnsAsync(respuestaEsperada);

            var result = await _controller.Login(request);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(200, okResult.StatusCode);

            var infoAutenticacion = Assert.IsType<AuthResponseDto>(okResult.Value);
            Assert.Equal("jwt.token.simulado", infoAutenticacion.Token);
        }
        
        /// <summary>
        /// Valida que el endpoint retorne un código 401 (Unauthorized) cuando el servicio de autenticación falla.
        /// </summary>
        [Fact]
        public async Task Login_DeberiaRetornarUnauthorized_CuandoLasCredencialesSonIncorrectas()
        {
            var request = new LoginRequest
            {
                Username = "usuario_incorrecto",
                Password = "clave_incorrecta"
            };

            _authServiceMock
                .Setup(s => s.LoginAsync(request.Username, request.Password))
                .ReturnsAsync((AuthResponseDto)null);

            var result = await _controller.Login(request);

            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal(401, unauthorizedResult.StatusCode);
            Assert.NotNull(unauthorizedResult.Value);
        }
    }
}
