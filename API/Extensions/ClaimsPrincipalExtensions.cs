using System.Security.Claims;

namespace API.Extensions;

/// <summary>
/// Extiende ClaimsPrincipal para leer los claims del JWT de forma tipada.
/// Reemplaza los helpers temporales X-Usuario-Id / X-Rol-Id en los controllers.
/// </summary>
public static class ClaimsPrincipalExtensions {
    /// <summary>Id interno del usuario (int). Nunca se expone al frontend.</summary>
    public static int GetUsuarioId(this ClaimsPrincipal user)
        => int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException("Claim NameIdentifier no encontrado."));

    /// <summary>Identificador público del usuario (Guid). Es el que va en las URLs.</summary>
    public static Guid GetPublicId(this ClaimsPrincipal user)
        => Guid.Parse(user.FindFirstValue("PublicId")
            ?? throw new InvalidOperationException("Claim PublicId no encontrado."));

    /// <summary>Id del rol (int). 1=Admin, 2=Técnico, 3=Solicitante.</summary>
    public static int GetRolId(this ClaimsPrincipal user)
        => int.Parse(user.FindFirstValue("RolId")
            ?? throw new InvalidOperationException("Claim RolId no encontrado."));

    /// <summary>Nombre del rol como string.</summary>
    public static string GetRol(this ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.Role)
            ?? throw new InvalidOperationException("Claim Role no encontrado.");

    /// <summary>Email del usuario autenticado.</summary>
    public static string GetEmail(this ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.Email)
            ?? throw new InvalidOperationException("Claim Email no encontrado.");

    /// <summary>Nombre completo del usuario autenticado.</summary>
    public static string GetNombre(this ClaimsPrincipal user)
        => user.FindFirstValue(ClaimTypes.Name)
            ?? throw new InvalidOperationException("Claim Name no encontrado.");
}
