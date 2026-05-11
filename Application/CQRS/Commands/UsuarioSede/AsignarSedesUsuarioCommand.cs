using Application.CQRS.Core;
using Application.Exceptions;
using Application.Ports.Driven;

namespace Application.CQRS.Commands.UsuarioSede;

public record AsignarSedesUsuarioCommand(Guid PublicId, List<int> SedeIds) : ICommand;

public class AsignarSedesUsuarioHandler : ICommandHandler<AsignarSedesUsuarioCommand> {
    private readonly IUsuarioRepository _usuarioRepo;
    private readonly IUsuarioSedeRepository _usuarioSedeRepo;

    public AsignarSedesUsuarioHandler(IUsuarioRepository usuarioRepo, IUsuarioSedeRepository usuarioSedeRepo) {
        _usuarioRepo = usuarioRepo;
        _usuarioSedeRepo = usuarioSedeRepo;
    }

    public async Task<Unit> HandleAsync(AsignarSedesUsuarioCommand cmd, CancellationToken ct = default) {
        var usuario = await _usuarioRepo.ObtenerPorPublicIdAsync(cmd.PublicId, ct)
            ?? throw new NotFoundException("Usuario", cmd.PublicId);

        await _usuarioSedeRepo.AsignarSedesAsync(usuario.UsuarioId, cmd.SedeIds, ct);
        return Unit.Value;
    }
}
