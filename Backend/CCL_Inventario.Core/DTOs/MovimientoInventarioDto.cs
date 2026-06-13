namespace CCL_Inventario.Core.Dtos
{
    public class MovimientoInventarioDto
    {
        public int Id { get; set; }
        public string TipoMovimiento { get; set; } = string.Empty;
        public int Cantidad { get; set; }
    }
}