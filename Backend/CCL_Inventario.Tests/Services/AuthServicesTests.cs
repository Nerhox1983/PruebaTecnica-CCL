using Moq;
using Microsoft.Extensions.Configuration;
using CCL_Inventario.Core.Services;

namespace CCL_Inventario.Tests
{
    /// <summary>
    /// Pruebas unitarias para la lógica de autenticación y generación de tokens.
    /// </summary>
    public class AuthServiceTests
    {
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IConfigurationSection> _jwtSectionMock;
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _configurationMock = new Mock<IConfiguration>();
            _jwtSectionMock = new Mock<IConfigurationSection>();

            _configurationMock
                .Setup(c => c["TestCredentials:Username"])
                .Returns("admin");

            _configurationMock
                .Setup(c => c["TestCredentials:Password"])
                .Returns("Secret123*");

            _jwtSectionMock.Setup(s => s["SecretKey"]).Returns("SuperSecretKeyDeMasDeTreintaYDosCaracteresLongitud!");
            _jwtSectionMock.Setup(s => s["Issuer"]).Returns("CCL_Inventario_Issuer");
            _jwtSectionMock.Setup(s => s["Audience"]).Returns("CCL_Inventario_Audience");

            _configurationMock
                .Setup(c => c.GetSection("JwtSettings"))
                .Returns(_jwtSectionMock.Object);

            _authService = new AuthService(_configurationMock.Object);
        }

        /// <summary>
        /// Valida que el login retorne null cuando se proporcionan credenciales que no coinciden con la configuración.
        /// </summary>
        [Fact]
        public async Task LoginAsync_DeberiaRetornarNull_CuandoLasCredencialesSonIncorrectas()
        {
            var resultado = await _authService.LoginAsync("usuario_invalido", "clave_incorrecta");

            Assert.Null(resultado);
        }

        /// <summary>
        /// Verifica la generación correcta de un token JWT y sus propiedades cuando el login es exitoso.
        /// </summary>
        [Fact]
        public async Task LoginAsync_DeberiaRetornarToken_CuandoLasCredencialesSonCorrectas()
        {
            var resultado = await _authService.LoginAsync("admin", "Secret123*");

            Assert.NotNull(resultado);
            Assert.Equal("Bearer", resultado.Type);
            Assert.Equal(3600, resultado.ExpiresIn);
            Assert.Equal("Autenticación exitosa", resultado.Message);
            Assert.False(string.IsNullOrWhiteSpace(resultado.Token));
        }
    }
}