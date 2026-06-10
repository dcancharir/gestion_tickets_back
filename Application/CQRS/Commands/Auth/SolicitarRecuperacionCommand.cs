using Application.CQRS.Core;
using Application.Exceptions;
using Application.Ports.Driven;
using System.Security.Cryptography;

namespace Application.CQRS.Commands.Auth;

// ── Command ───────────────────────────────────────────────────────────────────
// El administrador llama a este comando para generar un token de recuperación
// y enviar el correo al usuario indicado (por su PublicId).

public record SolicitarRecuperacionCommand(Guid UsuarioPublicId) : ICommand<bool>;

// ── Handler ───────────────────────────────────────────────────────────────────

public class SolicitarRecuperacionHandler
    : ICommandHandler<SolicitarRecuperacionCommand, bool> {

    private readonly IUsuarioRepository _repo;
    private readonly IEmailService      _email;
    private readonly IAppSettings       _settings;

    public SolicitarRecuperacionHandler(
        IUsuarioRepository repo,
        IEmailService      email,
        IAppSettings       settings) {
        _repo     = repo;
        _email    = email;
        _settings = settings;
    }

    public async Task<bool> HandleAsync(
        SolicitarRecuperacionCommand command,
        CancellationToken ct = default) {

        // 1. Obtener usuario
        var usuario = await _repo.ObtenerPorPublicIdAsync(command.UsuarioPublicId, ct)
            ?? throw new NotFoundException("Usuario", command.UsuarioPublicId);

        if (!usuario.Activo)
            throw new ConflictException("La cuenta del usuario está desactivada.");

        // 2. Generar token criptográficamente seguro (32 bytes → 64 hex chars)
        var tokenBytes = RandomNumberGenerator.GetBytes(32);
        var token      = Convert.ToHexString(tokenBytes).ToLowerInvariant();

        // 3. Persistir token y expiración (2 horas)
        usuario.TokenRecuperacion = token;
        usuario.TokenExpiracion   = DateTime.UtcNow.AddHours(2);
        await _repo.ActualizarAsync(usuario, ct);

        // 4. Armar link de recuperación
        var link = $"{_settings.FrontendUrl}/restablecer-password?token={token}";

        // 5. Enviar correo HTML
        var asunto = "Restablecer contraseña — Sistema de Tickets";
        var cuerpo = $"""
            <!DOCTYPE html>
            <html lang="es">
            <body style="margin:0;padding:0;background:#F7F5F1;font-family:'Segoe UI',system-ui,sans-serif;">
              <table width="100%" cellpadding="0" cellspacing="0">
                <tr><td align="center" style="padding:40px 16px;">
                  <table width="520" cellpadding="0" cellspacing="0"
                         style="background:#fff;border:1px solid #E8E3DC;border-radius:16px;overflow:hidden;">
                    <!-- Header naranja -->
                    <tr><td style="background:#ED5A1C;padding:28px 32px;">
                      <span style="color:#fff;font-size:20px;font-weight:700;letter-spacing:-.5px;">
                        🔐 Restablecer contraseña
                      </span>
                    </td></tr>
                    <!-- Cuerpo -->
                    <tr><td style="padding:32px;">
                      <p style="margin:0 0 12px;color:#2E2A26;font-size:15px;">
                        Hola, <strong>{usuario.Nombre} {usuario.Apellidos}</strong>
                      </p>
                      <p style="margin:0 0 24px;color:#524C45;font-size:14px;line-height:1.6;">
                        Un administrador ha solicitado el restablecimiento de tu contraseña.<br>
                        Haz clic en el botón para crear una nueva contraseña.
                      </p>
                      <a href="{link}"
                         style="display:inline-block;background:#ED5A1C;color:#fff;
                                font-size:14px;font-weight:600;padding:12px 28px;
                                border-radius:10px;text-decoration:none;">
                        Restablecer mi contraseña
                      </a>
                      <p style="margin:24px 0 0;color:#8A827A;font-size:12px;">
                        Este enlace es válido por <strong>2 horas</strong> y solo puede usarse una vez.<br>
                        Si no solicitaste esto, ignora este correo.
                      </p>
                    </td></tr>
                    <!-- Footer -->
                    <tr><td style="background:#FAF8F4;padding:16px 32px;border-top:1px solid #E8E3DC;">
                      <p style="margin:0;color:#A9A199;font-size:11px;">
                        Sistema de Gestión de Tickets — Soporte Remoto SAC
                      </p>
                    </td></tr>
                  </table>
                </td></tr>
              </table>
            </body>
            </html>
            """;

        await _email.SendEmail(usuario.Email, asunto, cuerpo, isHtml: true);
        return true;
    }
}
