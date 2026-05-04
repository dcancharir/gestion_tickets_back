using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Dashboard;

public record KpiTecnicoDto(
    string Tecnico,
    int TotalAsignados,
    int Resueltos,
    int Cerrados,
    double? MttrPromedioMinutos,
    double? PorcentajeCumplimientoSla,
    double? PorcentajePrimerContacto
);
