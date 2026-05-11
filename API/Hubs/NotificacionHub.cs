using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

[Authorize]
public class NotificacionHub : Hub {
    // El servidor envía al cliente; los clientes no invocan métodos del hub.
    // La identificación del usuario se hace automáticamente por
    // ClaimTypes.NameIdentifier (= UsuarioId) desde el JWT.
}
