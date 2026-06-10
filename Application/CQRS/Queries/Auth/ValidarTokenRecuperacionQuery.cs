using Application.CQRS.Core;
using Application.Ports.Driven;

namespace Application.CQRS.Queries.Auth;

// ── Query ─────────────────────────────────────────────────────────────────────
// El frontend lo llama al cargar la página de restablecimiento para verificar
// que el token existe y no ha expirado antes de mostrar el formulario.

public record ValidarTokenRecuperacionQuery(string Token) : IQuery<TokenValidoDto>;

public record TokenValidoDto(bool Valido, string? NombreUsuario);

// ── Handler ───────────────────────────────────────────────────────────────────

public class ValidarTokenRecuperacionHandler
    : IQueryHandler<ValidarTokenRecuperacionQuery, TokenValidoDto> {

    private readonly IUsuarioRepository _repo;

    public ValidarTokenRecuperacionHandler(IUsuarioRepository repo) => _repo = repo;

    public async Task<TokenValidoDto> HandleAsync(
        ValidarTokenRecuperacionQuery query,
        CancellationToken ct = default) {

        var usuario = await _repo.ObtenerPorTokenRecuperacionAsync(query.Token, ct);

        if (usuario is null
            || usuario.TokenExpiracion is null
            || usuario.TokenExpiracion < DateTime.UtcNow)
            return new TokenValidoDto(false, null);

        return new TokenValidoDto(true, $"{usuario.Nombre} {usuario.Apellidos}");
    }
}
