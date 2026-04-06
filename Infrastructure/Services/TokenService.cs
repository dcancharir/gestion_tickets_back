using Application.Ports.Driven;
using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Services;

public class TokenService : ITokenService {
    private readonly IConfiguration _config;

    public TokenService(IConfiguration config) => _config = config;

    public string GenerarToken(Usuario usuario) {
        var clave = _config["Jwt:Clave"]!;
        var emisor = _config["Jwt:Emisor"]!;
        var audiencia = _config["Jwt:Audiencia"]!;
        var duracion = int.Parse(_config["Jwt:DuracionHoras"] ?? "8");
        var claveBytes = Encoding.UTF8.GetBytes(clave);
        var credenciales = new SigningCredentials(
            new SymmetricSecurityKey(claveBytes),
            SecurityAlgorithms.HmacSha256);

        // Claims embebidos en el token — leídos en los controllers con User.FindFirstValue()
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.UsuarioId.ToString()),
            new Claim("PublicId",                usuario.PublicId.ToString()),
            new Claim(ClaimTypes.Email,          usuario.Email),
            new Claim(ClaimTypes.Name,           $"{usuario.Nombre} {usuario.Apellidos}"),
            new Claim("RolId",                   usuario.RolId.ToString()),
            new Claim(ClaimTypes.Role,           usuario.Rol.Nombre),
            new Claim("UserName",                   usuario.UserName),
        };

        var token = new JwtSecurityToken(
            issuer: emisor,
            audience: audiencia,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(duracion),
            signingCredentials: credenciales
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public int? ObtenerUsuarioId(string token) {
        try {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            var claim = jwt.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return claim is not null ? int.Parse(claim.Value) : null;
        } catch { return null; }
    }
}
