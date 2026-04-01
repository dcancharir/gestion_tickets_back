using API.Filters;
using API.Middlewares;
using Application.CQRS.Core;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ── Infraestructura (DbContext + Repositorios + Servicios) ────────────────────
builder.Services.AddInfrastructure(builder.Configuration);

// ── CQRS (Dispatcher + todos los handlers automáticamente) ────────────────────
builder.Services.AddCqrs();


// ── JWT Authentication ────────────────────────────────────────────────────────
var claveJwt = builder.Configuration["Jwt:Clave"]
    ?? throw new InvalidOperationException("Jwt:Clave no configurado en appsettings.");

builder.Services
    .AddAuthentication(options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Emisor"],
            ValidAudience = builder.Configuration["Jwt:Audiencia"],
            IssuerSigningKey = new SymmetricSecurityKey(
                                           Encoding.UTF8.GetBytes(claveJwt)),
            ClockSkew = TimeSpan.Zero  // sin margen de tolerancia en expiración
        };

        // Devolver 401 como JSON en lugar de redireccionar
        options.Events = new JwtBearerEvents {
            OnChallenge = async context => {
                context.HandleResponse();
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    "{\"status\":401,\"mensaje\":\"No autorizado. Token inválido o expirado.\"}");
            },
            OnForbidden = async context => {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    "{\"status\":403,\"mensaje\":\"No tienes permisos para realizar esta acción.\"}");
            }
        };
    });

builder.Services.AddAuthorization();

// ── CQRS — registra Dispatcher + todos los handlers automáticamente ───────────
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();

// ── Swagger con soporte para JWT ──────────────────────────────────────────────
builder.Services.AddSwaggerGen(opt => {
    opt.SwaggerDoc("v1", new OpenApiInfo {
        Title = "SistemaTickets API",
        Version = "v1",
        Description = "API para gestión de wallet incidencias de la empresa Soporte Remoto SAC"
    });

    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa el token JWT. Ejemplo: Bearer {tu_token}"
    });

    opt.OperationFilter<SecurityRequirementsOperationFilter>();
});


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
    app.UseSwagger();
    app.UseSwaggerUI(opt => {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "SistemaTickets API v1");
        opt.RoutePrefix = "swagger"; // acceso en /swagger
        opt.DocumentTitle = "SistemaTickets API - Docs";
        opt.DefaultModelsExpandDepth(-1); // oculta sección Schemas por defecto
    });
}

app.UseHttpsRedirection();
app.UseCors("Angular");
app.UseAuthentication();  // antes de UseAuthorization

app.UseAuthorization();

app.MapControllers();

app.Run();
