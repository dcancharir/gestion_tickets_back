namespace Application.DTOS.Dashboard;

/// <summary>Datos de un día para el gráfico de tendencia.</summary>
public record TendenciaDiaDto(
    string Fecha,       // "dd/MM"
    int    Registrados,
    int    Resueltos
);
