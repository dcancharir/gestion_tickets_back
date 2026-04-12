using System;
using System.Collections.Generic;
using System.Text;

namespace Application.DTOS.Sedes;

public record SedeDto(int SedeId,int SedeIdExterno,string Nombre, string TipoSede);
