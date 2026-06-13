using CCL_Inventario.Core.DTOs;

namespace CCL_Inventario.Core.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(string username, string password);
    }
}