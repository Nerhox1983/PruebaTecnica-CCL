namespace CCL_Inventario.Core.DTOs
{
    public class AuthResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Type { get; set; } = "Bearer";
        public int ExpiresIn { get; set; } = 3600;
        public string Message { get; set; } = string.Empty;
    }
}