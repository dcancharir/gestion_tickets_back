using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
namespace Application.DTOS.Incidencias;

public record RegistrarIncidenciaDto(
    string Titulo,
    string Descripcion,
    int CategoriaId,
    string CanalReporte,
    byte Impacto,
    byte Urgencia,
    int PrioridadId,
    int SedeId,
    List<IFormFile>? Adjuntos
// SolicitanteId viene del JWT, no del body
);
