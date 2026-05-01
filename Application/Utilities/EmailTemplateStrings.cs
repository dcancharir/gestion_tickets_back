using Application.DTOS.Incidencias;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Utilities;

public static class EmailTemplateStrings {
    public static string NewIncidenciaTemplate(IncidenciaListItemDto incidencia, string uriSistema) {
        return $@"
    <!DOCTYPE html>
<html>
<head>
  <meta charset=""UTF-8"">
  <title>Verify Email</title>
</head>
<body style=""margin:0; padding:0; background-color:#fff;"">
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#f4f6f9; padding:40px 0;"">
    <tr>
      <td align=""center"">
        <table width=""500"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#ffffff; border-radius:8px; padding:40px; font-family:Arial, sans-serif; color:#1a1c23;"">
          
          <!-- Logo / Header -->
          <tr>
            <td align=""center"" style=""padding-bottom:20px;"">
              <span style=""font-size:24px; font-weight:bold; color:#1a1c23;"">Incidencia Registrada</span>
            </td>
          </tr>

          <!-- Title -->
          <tr>
            <td align=""center"" style=""font-size:20px; font-weight:bold; padding-bottom:10px; color:#1a1c23;"">
              Nro. {incidencia.NumeroTicket}
            </td>
          </tr>

          <!-- Text -->
          <tr>
            <td align=""left"" style=""font-size:14px; line-height:1.6; color:#000; padding-bottom:30px;"">
                <span style=""font-weight:bold"">INCIDENCIA : </span>{incidencia.Titulo}<br>
                <span style=""font-weight:bold"">SALA : </span>{incidencia.Sede}<br>
                <span style=""font-weight:bold"">FECHA REGISTRO : </span>{incidencia.FechaRegistro.ToString("yyyy-MM-dd HH:mm:ss tt")}<br>
                <span style=""font-weight:bold"">CATEGORIA : </span>{incidencia.Categoria}<br>
                  <span style=""font-weight:bold"">PRIORIDAD : </span>{incidencia.Prioridad}
           
            </td>
          </tr>
          <!-- Footer -->
          <tr>
            <td align=""left"" style=""font-size:13px; color:#000;"">
              Puede ingresar ver la incidencia en el siguiente enlace<br/><br/>
              <a href=""{uriSistema}"" style=""color:#blue; text-decoration:none;"">Sistema de Gestion de Incidencias</a>
            </td>
          </tr>

        </table>
      </td>
    </tr>
  </table>
</body>
</html>
";
    }
}
