using CCL_Inventario.Api.Requests;
using CCL_Inventario.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CCL_Inventario.Api.Controllers;

/// <summary>
/// Controlador encargado de gestionar los procesos de autenticación y seguridad.
/// </summary>
[ApiController]
[Route("auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>
    /// Valida las credenciales del usuario y genera un token de acceso JWT.
    /// </summary>
    /// <param name="request">Objeto que contiene el nombre de usuario y la contraseña.</param>
    /// <returns>Un token Bearer y la información de expiración si las credenciales son válidas.</returns>
    /// <response code="200">Retorna el token de autenticación exitosamente.</response>
    /// <response code="401">Si las credenciales proporcionadas son incorrectas.</response>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await authService.LoginAsync(request.Username, request.Password);

        if (result == null)
        {
            return Unauthorized(new { Message = "Credenciales incorrectas" });
        }

        return Ok(result);
    }
}