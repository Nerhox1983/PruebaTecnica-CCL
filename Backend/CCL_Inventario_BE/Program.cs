using System.Diagnostics.CodeAnalysis; // Asegúrate de incluir este using arriba
using CCL_Inventario.Core.Interfaces;
using CCL_Inventario.Core.Repositories;
using CCL_Inventario.Core.Services;
using CCL_Inventario.Infrastructure.Data;
using CCL_Inventario.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==========================================
// === INICIO CORS: Configurar el servicio ===
// ==========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularAppPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // El puerto nativo de tu Front-end
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// ==========================================
// === FIN CORS: Configurar el servicio ===
// ==========================================

var connectionString = builder.Configuration.GetConnectionString("PostgresConnection");
builder.Services.AddDbContext<InventarioDbContext>(options =>
    options.UseNpgsql(connectionString, b => b.MigrationsAssembly("CCL_Inventario.Infrastructure")));

builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductoService, ProductoService>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ==========================================
// === INICIO CORS: Habilitar Middleware ===
// ==========================================
// IMPORTANTE: Debe ir estrictamente ANTES de UseAuthentication y UseAuthorization
app.UseCors("AngularAppPolicy");
// ==========================================
// === FIN CORS: Habilitar Middleware ===
// ==========================================

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

[ExcludeFromCodeCoverage]
public partial class Program
{
}