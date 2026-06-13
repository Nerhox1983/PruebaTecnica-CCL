using CCL_Inventario.Core.Repositories;
using CCL_Inventario.Infrastructure.Data;
using CCL_Inventario.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");

    builder.Services.AddDbContext<InventarioDbContext>(options =>
        options.UseNpgsql(connectionString, b =>
            b.MigrationsAssembly("CCL_Inventario.Infrastructure")));

    builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
}