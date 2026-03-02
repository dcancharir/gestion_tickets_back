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
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
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
       
        // ── Servicios ─────────────────────────────────────────────────────────
        //services.AddScoped<ITokenService, TokenService>();
    }
}
