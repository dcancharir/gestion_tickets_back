using API.Middlewares;
using Application.CQRS.Core;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// ── Infraestructura (DbContext + Repositorios + Servicios) ────────────────────
builder.Services.AddInfrastructure(builder.Configuration);

// ── CQRS (Dispatcher + todos los handlers automáticamente) ────────────────────
builder.Services.AddCqrs();

// ── Base de datos ─────────────────────────────────────────────────────────────
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
// ── CQRS — registra Dispatcher + todos los handlers automáticamente ───────────
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// ── CORS para Angular ─────────────────────────────────────────────────────────
builder.Services.AddCors(options => {
    options.AddPolicy("Angular", policy =>
        policy.WithOrigins()
              .AllowAnyHeader()
              .AllowAnyMethod().AllowAnyOrigin());
});

var app = builder.Build();

// ── Pipeline HTTP ─────────────────────────────────────────────────────────────
app.UseMiddleware<ExceptionMiddleware>(); // primero, para capturar todo


// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
    app.MapOpenApi(pattern:"swagger/openapi/v1.json");
    app.UseSwaggerUI(c => c.SwaggerEndpoint("openapi/v1.json","MyApi V1"));
}

app.UseHttpsRedirection();
app.UseCors("Angular");

app.UseAuthorization();

app.MapControllers();

app.Run();
