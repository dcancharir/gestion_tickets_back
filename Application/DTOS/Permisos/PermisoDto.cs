using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Permisos;

public record PermisoDto (int PermisoId,string Nombre, string Tipo, string Controlador);
