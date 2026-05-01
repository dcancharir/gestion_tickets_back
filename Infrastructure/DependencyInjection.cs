using Application.Ports.Driven;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure;

public static class DependencyInjection {
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
        // ── Base de datos ─────────────────────────────────────────────────────
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                sql => sql.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        // ── Repositorios ──────────────────────────────────────────────────────
        services.AddScoped<IRolRepository, RolRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<INivelPrioridadRepository, NivelPrioridadRepository>();
        services.AddScoped<IEstadoIncidenciaRepository, EstadoIncidenciaRepository>();
        services.AddScoped<IAcuerdoNivelServicioRepository, AcuerdoNivelServicioRepository>();
        services.AddScoped<IIncidenciaRepository, IncidenciaRepository>();
        services.AddScoped<IBaseConocimientoRepository, BaseConocimientoRepository>();
        services.AddScoped<IPermisoRepository, PermisoRepository>();
        services.AddScoped<IPermisoRolRepository, PermisoRolRepository>();
        services.AddScoped<ISedeRepository, SedeRepository>();
        services.AddScoped<IIncidenciaAdjuntoRepository, IncidenciaAdjuntoRepository>();
        // ── Servicios ─────────────────────────────────────────────────────────
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IPasswordGenerator, PasswordGenerator>();
        services.AddScoped<IFileStorageService, FileStorageService>();
        return services;
    }
}
