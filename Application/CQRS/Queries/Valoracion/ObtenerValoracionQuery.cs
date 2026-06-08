using Application.CQRS.Core;
using Application.DTOS.Valoracion;
using Application.Ports.Driven;

namespace Application.CQRS.Queries.Valoracion;

public record ObtenerValoracionQuery(Guid TicketPublicId) : IQuery<ValoracionDto?>;

public class ObtenerValoracionHandler
    : IQueryHandler<ObtenerValoracionQuery, ValoracionDto?> {

    private readonly IValoracionRepository _repo;

    public ObtenerValoracionHandler(IValoracionRepository repo) => _repo = repo;

    public async Task<ValoracionDto?> HandleAsync(
        ObtenerValoracionQuery query,
        CancellationToken ct = default) {

        var v = await _repo.ObtenerPorIncidenciaPublicIdAsync(query.TicketPublicId, ct);
        if (v is null) return null;

        return new ValoracionDto(v.ValoracionId, v.Puntuacion, v.Comentario, v.FechaValoracion);
    }
}
