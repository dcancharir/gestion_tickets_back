using Application.DTOS.Incidencias;
using Application.DTOS.Usuarios;
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
  <title>GESTIÓN DE INCIDENCIAS</title>
</head>
<body style=""margin:0; padding:0; background-color:#fff;"">
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#f4f6f9; padding:40px 0;"">
    <tr>
      <td align=""center"">
        <table width=""500"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#ffffff; border-radius:8px; padding:40px; font-family:Arial, sans-serif; color:#1a1c23;"">
          
          <!-- Logo / Header -->
          <tr>
            <td align=""center"" style=""padding-bottom:20px;"">
              <span style=""font-size:24px; font-weight:bold; color:#1a1c23;"">Sistema de Gestión de Incidencias</span>
            </td>
          </tr>

          <!-- Title -->
          <tr>
            <td align=""center"" style=""font-size:20px; font-weight:bold; padding-bottom:10px; color:#1a1c23;"">
              Se ha registrado una incidencia con el Nro. {incidencia.NumeroTicket}
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
    public static string TicketAsignadoTemplate(string tecnicoNombre, string numeroTicket, string titulo, string categoria, string prioridad, string uriSistema) {
        return $@"
<!DOCTYPE html>
<html>
<head><meta charset=""UTF-8""><title>Ticket Asignado</title></head>
<body style=""margin:0;padding:0;background-color:#fff;"">
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#f4f6f9;padding:40px 0;"">
    <tr><td align=""center"">
      <table width=""500"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#ffffff;border-radius:8px;padding:40px;font-family:Arial,sans-serif;color:#1a1c23;"">
        <tr><td align=""center"" style=""padding-bottom:20px;"">
          <span style=""font-size:24px;font-weight:bold;"">Sistema de Gestión de Incidencias</span>
        </td></tr>
        <tr><td align=""center"" style=""font-size:20px;font-weight:bold;padding-bottom:10px;color:#d7691c;"">
          🔧 Se te ha asignado un ticket
        </td></tr>
        <tr><td align=""left"" style=""font-size:14px;line-height:1.8;color:#000;padding-bottom:30px;"">
          Hola <strong>{tecnicoNombre}</strong>,<br/><br/>
          Se te ha asignado el siguiente ticket para su atención:<br/><br/>
          <strong>Ticket:</strong> {numeroTicket}<br/>
          <strong>Título:</strong> {titulo}<br/>
          <strong>Categoría:</strong> {categoria}<br/>
          <strong>Prioridad:</strong> {prioridad}
        </td></tr>
        <tr><td align=""left"" style=""font-size:13px;color:#000;"">
          Ver el ticket: <a href=""{uriSistema}"" style=""color:#d7691c;"">Abrir en el sistema</a>
        </td></tr>
      </table>
    </td></tr>
  </table>
</body>
</html>";
    }

    public static string TicketResueltoTemplate(string solicitanteNombre, string numeroTicket, string titulo, string solucion, string uriSistema) {
        return $@"
<!DOCTYPE html>
<html>
<head><meta charset=""UTF-8""><title>Ticket Resuelto</title></head>
<body style=""margin:0;padding:0;background-color:#fff;"">
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#f4f6f9;padding:40px 0;"">
    <tr><td align=""center"">
      <table width=""500"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#ffffff;border-radius:8px;padding:40px;font-family:Arial,sans-serif;color:#1a1c23;"">
        <tr><td align=""center"" style=""padding-bottom:20px;"">
          <span style=""font-size:24px;font-weight:bold;"">Sistema de Gestión de Incidencias</span>
        </td></tr>
        <tr><td align=""center"" style=""font-size:20px;font-weight:bold;padding-bottom:10px;color:#2ea44f;"">
          ✅ Tu ticket ha sido resuelto
        </td></tr>
        <tr><td align=""left"" style=""font-size:14px;line-height:1.8;color:#000;padding-bottom:30px;"">
          Hola <strong>{solicitanteNombre}</strong>,<br/><br/>
          Tu ticket ha sido resuelto:<br/><br/>
          <strong>Ticket:</strong> {numeroTicket}<br/>
          <strong>Título:</strong> {titulo}<br/><br/>
          <strong>Solución aplicada:</strong><br/>
          {solucion}<br/><br/>
          Si la solución fue satisfactoria, puedes cerrar el ticket desde el sistema.
          Si el problema persiste, puedes reabrirlo.
        </td></tr>
        <tr><td align=""left"" style=""font-size:13px;color:#000;"">
          Ver el ticket: <a href=""{uriSistema}"" style=""color:#2ea44f;"">Abrir en el sistema</a>
        </td></tr>
      </table>
    </td></tr>
  </table>
</body>
</html>";
    }

    public static string TicketCerradoTemplate(string solicitanteNombre, string numeroTicket, string titulo, string uriSistema) {
        return $@"
<!DOCTYPE html>
<html>
<head><meta charset=""UTF-8""><title>Ticket Cerrado</title></head>
<body style=""margin:0;padding:0;background-color:#fff;"">
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#f4f6f9;padding:40px 0;"">
    <tr><td align=""center"">
      <table width=""500"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#ffffff;border-radius:8px;padding:40px;font-family:Arial,sans-serif;color:#1a1c23;"">
        <tr><td align=""center"" style=""padding-bottom:20px;"">
          <span style=""font-size:24px;font-weight:bold;"">Sistema de Gestión de Incidencias</span>
        </td></tr>
        <tr><td align=""center"" style=""font-size:20px;font-weight:bold;padding-bottom:10px;color:#1a1c23;"">
          🔒 Tu ticket ha sido cerrado
        </td></tr>
        <tr><td align=""left"" style=""font-size:14px;line-height:1.8;color:#000;padding-bottom:30px;"">
          Hola <strong>{solicitanteNombre}</strong>,<br/><br/>
          El siguiente ticket ha sido cerrado:<br/><br/>
          <strong>Ticket:</strong> {numeroTicket}<br/>
          <strong>Título:</strong> {titulo}<br/><br/>
          Gracias por usar nuestro sistema de soporte.
        </td></tr>
        <tr><td align=""left"" style=""font-size:13px;color:#000;"">
          Ver historial: <a href=""{uriSistema}"" style=""color:#1a1c23;"">Abrir en el sistema</a>
        </td></tr>
      </table>
    </td></tr>
  </table>
</body>
</html>";
    }

    public static string SlaIncumplidoTemplate(string tecnicoNombre, string numeroTicket, string titulo, DateTime fechaLimite, string uriSistema) {
        return $@"
<!DOCTYPE html>
<html>
<head><meta charset=""UTF-8""><title>SLA Incumplido</title></head>
<body style=""margin:0;padding:0;background-color:#fff;"">
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#f4f6f9;padding:40px 0;"">
    <tr><td align=""center"">
      <table width=""500"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#ffffff;border-radius:8px;padding:40px;font-family:Arial,sans-serif;color:#1a1c23;"">
        <tr><td align=""center"" style=""padding-bottom:20px;"">
          <span style=""font-size:24px;font-weight:bold;"">Sistema de Gestión de Incidencias</span>
        </td></tr>
        <tr><td align=""center"" style=""font-size:20px;font-weight:bold;padding-bottom:10px;color:#c0392b;"">
          ⚠️ SLA Incumplido — Acción Requerida
        </td></tr>
        <tr><td align=""left"" style=""font-size:14px;line-height:1.8;color:#000;padding-bottom:30px;"">
          Hola <strong>{tecnicoNombre}</strong>,<br/><br/>
          El siguiente ticket ha superado su tiempo límite de resolución establecido por el SLA:<br/><br/>
          <strong>Ticket:</strong> {numeroTicket}<br/>
          <strong>Título:</strong> {titulo}<br/>
          <strong>Fecha límite SLA:</strong> {fechaLimite:yyyy-MM-dd HH:mm}<br/><br/>
          Por favor, atiende este ticket con la mayor urgencia posible y actualiza su estado.
        </td></tr>
        <tr><td align=""left"" style=""font-size:13px;color:#000;"">
          Ver el ticket: <a href=""{uriSistema}"" style=""color:#c0392b;"">Abrir en el sistema</a>
        </td></tr>
      </table>
    </td></tr>
  </table>
</body>
</html>";
    }

    public static string SlaIncumplidoSinTecnicoTemplate(string adminNombre, string numeroTicket, string titulo, DateTime fechaLimite, string uriSistema) {
        return $@"
<!DOCTYPE html>
<html>
<head><meta charset=""UTF-8""><title>SLA Vencido Sin Técnico</title></head>
<body style=""margin:0;padding:0;background-color:#fff;"">
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#f4f6f9;padding:40px 0;"">
    <tr><td align=""center"">
      <table width=""500"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#ffffff;border-radius:8px;padding:40px;font-family:Arial,sans-serif;color:#1a1c23;"">
        <tr><td align=""center"" style=""padding-bottom:20px;"">
          <span style=""font-size:24px;font-weight:bold;"">Sistema de Gestión de Incidencias</span>
        </td></tr>
        <tr><td align=""center"" style=""font-size:20px;font-weight:bold;padding-bottom:10px;color:#c0392b;"">
          ⚠️ SLA Vencido — Sin Técnico Asignado
        </td></tr>
        <tr><td align=""left"" style=""font-size:14px;line-height:1.8;color:#000;padding-bottom:30px;"">
          Hola <strong>{adminNombre}</strong>,<br/><br/>
          El siguiente ticket ha superado su tiempo límite de resolución y <strong>no tiene técnico asignado</strong>:<br/><br/>
          <strong>Ticket:</strong> {numeroTicket}<br/>
          <strong>Título:</strong> {titulo}<br/>
          <strong>Fecha límite SLA:</strong> {fechaLimite:yyyy-MM-dd HH:mm}<br/><br/>
          Por favor, asigna un técnico a este ticket de inmediato para atender el incumplimiento.
        </td></tr>
        <tr><td align=""left"" style=""font-size:13px;color:#000;"">
          Asignar técnico: <a href=""{uriSistema}"" style=""color:#c0392b;"">Abrir en el sistema</a>
        </td></tr>
      </table>
    </td></tr>
  </table>
</body>
</html>";
    }

    public static string NewUserTemplate(UsuarioDto usuario, string password, string uriSistema) {
        return $@"
    <!DOCTYPE html>
<html>
<head>
  <meta charset=""UTF-8"">
  <title>GESTIÓN DE INCIDENCIAS</title>
</head>
<body style=""margin:0; padding:0; background-color:#fff;"">
  <table width=""100%"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#f4f6f9; padding:40px 0;"">
    <tr>
      <td align=""center"">
        <table width=""500"" cellpadding=""0"" cellspacing=""0"" border=""0"" style=""background-color:#ffffff; border-radius:8px; padding:40px; font-family:Arial, sans-serif; color:#1a1c23;"">
          
          <!-- Logo / Header -->
          <tr>
            <td align=""center"" style=""padding-bottom:20px;"">
              <span style=""font-size:24px; font-weight:bold; color:#1a1c23;"">Sistema de Gestión de Incidencias</span>
            </td>
          </tr>

          <!-- Title -->
          <tr>
            <td align=""center"" style=""font-size:20px; font-weight:bold; padding-bottom:10px; color:#1a1c23;"">
              Se te ha registrado una nueva cuenta para poder acceder al sistema, para poder acceder al sistema usa las siguientes credenciales
            </td>
          </tr>

          <!-- Text -->
          <tr>
            <td align=""left"" style=""font-size:14px; line-height:1.6; color:#000; padding-bottom:30px;"">
                <span style=""font-weight:bold"">USUARIO : </span>{usuario.UserName}<br>
                <span style=""font-weight:bold"">PASSWORD : </span>{password}<br>
                <span style=""font-weight:bold"">FECHA REGISTRO : </span>{usuario.FechaCreacion.ToString("yyyy-MM-dd HH:mm:ss tt")}<br>
           
            </td>
          </tr>
          <!-- Footer -->
          <tr>
            <td align=""left"" style=""font-size:13px; color:#000;"">
              Puede ingresar al sistema a través del siguiente enlace :<br/><br/>
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
