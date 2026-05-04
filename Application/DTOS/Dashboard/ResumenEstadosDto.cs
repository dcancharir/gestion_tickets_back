using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Dashboard;

public record ResumenEstadosDto(
    int Total,
    int Registrados,
    int Asignados,
    int EnDiagnostico,
    int EnProgreso,
    int Pendientes,
    int Resueltos,
    int Cerrados,
    int Reabiertas,
    int Cancelados
);
