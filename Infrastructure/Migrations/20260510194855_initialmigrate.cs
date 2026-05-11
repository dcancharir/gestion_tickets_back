using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initialmigrate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.CategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "EstadosIncidencia",
                columns: table => new
                {
                    EstadoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    EsEstadoFinal = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    ColorHexa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CssClass = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EstadosIncidencia", x => x.EstadoId);
                });

            migrationBuilder.CreateTable(
                name: "NivelesPrioridad",
                columns: table => new
                {
                    PrioridadId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Impacto = table.Column<byte>(type: "tinyint", nullable: false),
                    Urgencia = table.Column<byte>(type: "tinyint", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Nivel = table.Column<byte>(type: "tinyint", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TiempoRespuestaMin = table.Column<int>(type: "int", nullable: false),
                    TiempoResolucionMin = table.Column<int>(type: "int", nullable: false),
                    ColorHexa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CssClass = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GuiaValor = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NivelesPrioridad", x => x.PrioridadId);
                    table.CheckConstraint("CK_NivelPrioridad_Nivel", "[Nivel] BETWEEN 1 AND 4");
                });

            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    PermisoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Controlador = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permisos", x => x.PermisoId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RolId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RolId);
                });

            migrationBuilder.CreateTable(
                name: "Sedes",
                columns: table => new
                {
                    SedeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SedeIdExterno = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    TipoSede = table.Column<string>(type: "nvarchar(250)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sedes", x => x.SedeId);
                });

            migrationBuilder.CreateTable(
                name: "AcuerdosNivelServicio",
                columns: table => new
                {
                    SlaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    PrioridadId = table.Column<int>(type: "int", nullable: false),
                    TiempoRespuestaMin = table.Column<int>(type: "int", nullable: false),
                    TiempoResolucionMin = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcuerdosNivelServicio", x => x.SlaId);
                    table.ForeignKey(
                        name: "FK_AcuerdosNivelServicio_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AcuerdosNivelServicio_NivelesPrioridad_PrioridadId",
                        column: x => x.PrioridadId,
                        principalTable: "NivelesPrioridad",
                        principalColumn: "PrioridadId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PermisosRol",
                columns: table => new
                {
                    PermisoRolId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PermisoId = table.Column<int>(type: "int", nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermisosRol", x => x.PermisoRolId);
                    table.ForeignKey(
                        name: "FK_PermisosRol_Permisos_PermisoId",
                        column: x => x.PermisoId,
                        principalTable: "Permisos",
                        principalColumn: "PermisoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermisosRol_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RolId = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    HasFullAccess = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.UsuarioId);
                    table.ForeignKey(
                        name: "FK_Usuarios_Roles_RolId",
                        column: x => x.RolId,
                        principalTable: "Roles",
                        principalColumn: "RolId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BaseConocimiento",
                columns: table => new
                {
                    ArticuloId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    CategoriaId = table.Column<int>(type: "int", nullable: true),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Problema = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Solucion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreadoPorId = table.Column<int>(type: "int", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Activo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseConocimiento", x => x.ArticuloId);
                    table.ForeignKey(
                        name: "FK_BaseConocimiento_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BaseConocimiento_Usuarios_CreadoPorId",
                        column: x => x.CreadoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Incidencias",
                columns: table => new
                {
                    IncidenciaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWSEQUENTIALID()"),
                    NumeroTicket = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Titulo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    CanalReporte = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Impacto = table.Column<byte>(type: "tinyint", nullable: false),
                    Urgencia = table.Column<byte>(type: "tinyint", nullable: false),
                    PrioridadId = table.Column<int>(type: "int", nullable: false),
                    SolicitanteId = table.Column<int>(type: "int", nullable: false),
                    TecnicoAsignadoId = table.Column<int>(type: "int", nullable: true),
                    FechaAsignacion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EstadoId = table.Column<int>(type: "int", nullable: false),
                    FechaLimiteRespuesta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaLimiteResolucion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaPrimeraRespuesta = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EscaladoAId = table.Column<int>(type: "int", nullable: true),
                    FechaEscalamiento = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NumeroReasignaciones = table.Column<byte>(type: "tinyint", nullable: false, defaultValue: (byte)0),
                    SolucionAplicada = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResueltoEnPrimerContacto = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    FechaResolucion = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FechaCierre = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CerradoPorId = table.Column<int>(type: "int", nullable: true),
                    FechaUltimaActualizacion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    SedeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidencias", x => x.IncidenciaId);
                    table.CheckConstraint("CK_Incidencia_Impacto", "[Impacto] BETWEEN 1 AND 3");
                    table.CheckConstraint("CK_Incidencia_Urgencia", "[Urgencia] BETWEEN 1 AND 3");
                    table.ForeignKey(
                        name: "FK_Incidencias_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "CategoriaId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidencias_EstadosIncidencia_EstadoId",
                        column: x => x.EstadoId,
                        principalTable: "EstadosIncidencia",
                        principalColumn: "EstadoId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidencias_NivelesPrioridad_PrioridadId",
                        column: x => x.PrioridadId,
                        principalTable: "NivelesPrioridad",
                        principalColumn: "PrioridadId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidencias_Sedes_SedeId",
                        column: x => x.SedeId,
                        principalTable: "Sedes",
                        principalColumn: "SedeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidencias_Usuarios_CerradoPorId",
                        column: x => x.CerradoPorId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidencias_Usuarios_EscaladoAId",
                        column: x => x.EscaladoAId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidencias_Usuarios_SolicitanteId",
                        column: x => x.SolicitanteId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incidencias_Usuarios_TecnicoAsignadoId",
                        column: x => x.TecnicoAsignadoId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioSede",
                columns: table => new
                {
                    UsuarioSedeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    SedeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioSede", x => x.UsuarioSedeId);
                    table.ForeignKey(
                        name: "FK_UsuarioSede_Sedes_SedeId",
                        column: x => x.SedeId,
                        principalTable: "Sedes",
                        principalColumn: "SedeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuarioSede_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComentariosIncidencia",
                columns: table => new
                {
                    ComentarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncidenciaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Mensaje = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EsInterno = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    FechaComentario = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComentariosIncidencia", x => x.ComentarioId);
                    table.ForeignKey(
                        name: "FK_ComentariosIncidencia_Incidencias_IncidenciaId",
                        column: x => x.IncidenciaId,
                        principalTable: "Incidencias",
                        principalColumn: "IncidenciaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComentariosIncidencia_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HistorialIncidencias",
                columns: table => new
                {
                    HistorialId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncidenciaId = table.Column<int>(type: "int", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Accion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EstadoAnterior = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EstadoNuevo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Detalle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaAccion = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistorialIncidencias", x => x.HistorialId);
                    table.ForeignKey(
                        name: "FK_HistorialIncidencias_Incidencias_IncidenciaId",
                        column: x => x.IncidenciaId,
                        principalTable: "Incidencias",
                        principalColumn: "IncidenciaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistorialIncidencias_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncidenciaAdjuntos",
                columns: table => new
                {
                    IncidenciaAdjuntoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncidenciaId = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(250)", nullable: false),
                    RutaContenedora = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreReal = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaCreacion = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidenciaAdjuntos", x => x.IncidenciaAdjuntoId);
                    table.ForeignKey(
                        name: "FK_IncidenciaAdjuntos_Incidencias_IncidenciaId",
                        column: x => x.IncidenciaId,
                        principalTable: "Incidencias",
                        principalColumn: "IncidenciaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "CategoriaId", "Activo", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, true, "Fallas en equipos físicos: computadoras, impresoras, periféricos", "Hardware" },
                    { 2, true, "Errores en aplicaciones, sistemas operativos o instalaciones", "Software" },
                    { 3, true, "Problemas de internet, red local, VPN o switches", "Red y Conectividad" },
                    { 4, true, "Virus, accesos no autorizados, vulnerabilidades", "Seguridad" },
                    { 5, true, "Fallos en cuentas de correo, Outlook o servidor de mail", "Correo Electrónico" },
                    { 6, true, "Errores en motores de base de datos o consultas", "Base de Datos" },
                    { 7, true, "Solicitudes de acceso, bloqueos de cuenta o cambios de contraseña", "Accesos y Permisos" },
                    { 8, true, "Incidencias que no corresponden a las categorías anteriores", "Otros" }
                });

            migrationBuilder.InsertData(
                table: "EstadosIncidencia",
                columns: new[] { "EstadoId", "ColorHexa", "CssClass", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "#6C757D", "secondary", "Incidencia recibida y pendiente de asignación", "Registrado" },
                    { 2, "#0D6EFD", "primary", "Asignado a un técnico, pendiente de atención", "Asignado" },
                    { 3, "#0DCAF0", "info", "El técnico está analizando la causa raíz", "En Diagnóstico" },
                    { 4, "#FFC107", "warning", "Se está aplicando la solución", "En Progreso" },
                    { 5, "#495057", "dark", "En espera de respuesta del solicitante o de un proveedor", "Pendiente" },
                    { 6, "#198754", "success", "Solución aplicada, pendiente de confirmación del usuario", "Resuelto" }
                });

            migrationBuilder.InsertData(
                table: "EstadosIncidencia",
                columns: new[] { "EstadoId", "ColorHexa", "CssClass", "Descripcion", "EsEstadoFinal", "Nombre" },
                values: new object[] { 7, "#ADB5BD", "secondary", "Confirmado y cerrado formalmente", true, "Cerrado" });

            migrationBuilder.InsertData(
                table: "EstadosIncidencia",
                columns: new[] { "EstadoId", "ColorHexa", "CssClass", "Descripcion", "Nombre" },
                values: new object[] { 8, "#DC3545", "danger", "El usuario reportó que el problema persiste", "Reabierto" });

            migrationBuilder.InsertData(
                table: "EstadosIncidencia",
                columns: new[] { "EstadoId", "ColorHexa", "CssClass", "Descripcion", "EsEstadoFinal", "Nombre" },
                values: new object[] { 9, "#E9ECEF", "light", "Incidencia anulada por el solicitante o administrador", true, "Cancelado" });

            migrationBuilder.InsertData(
                table: "NivelesPrioridad",
                columns: new[] { "PrioridadId", "ColorHexa", "CssClass", "Descripcion", "GuiaValor", "Impacto", "Nivel", "Nombre", "TiempoResolucionMin", "TiempoRespuestaMin", "Urgencia" },
                values: new object[,]
                {
                    { 1, "#B91C1C", "danger", "Impacto Alto + Urgencia Alta", "ATENCIÓN INMEDIATA - El valor del servicio está severamente comprometido. Asignar recursos prioritarios y mantener comunicación constante con stakeholders.", (byte)1, (byte)1, "Crítico", 60, 15, (byte)1 },
                    { 2, "#EA580C", "warning", "Impacto Alto + Urgencia Media", "ALTA PRIORIDAD - Valor del servicio significativamente afectado. Coordinar recursos y establecer plan de acción claro con tiempos definidos.", (byte)1, (byte)2, "Alto", 240, 30, (byte)2 },
                    { 3, "#EA580C", "warning", "Impacto Medio + Urgencia Alta", "ALTA PRIORIDAD - Requiere respuesta rápida aunque el impacto sea limitado. Asegurar que el usuario pueda continuar operaciones críticas.", (byte)2, (byte)2, "Alto", 240, 30, (byte)1 },
                    { 4, "#2563EB", "info", "Impacto Alto + Urgencia Baja", "PRIORIDAD MEDIA - Planificar resolución considerando impacto potencial. Puede coordinarse con otros cambios planificados.", (byte)1, (byte)3, "Medio", 480, 60, (byte)3 },
                    { 5, "#2563EB", "info", "Impacto Medio + Urgencia Media", "PRIORIDAD MEDIA - Gestión normal siguiendo procedimientos estándar. Mantener al usuario informado del progreso.", (byte)2, (byte)3, "Medio", 480, 60, (byte)2 },
                    { 6, "#2563EB", "info", "Impacto Bajo + Urgencia Alta", "PRIORIDAD MEDIA - Respuesta rápida con bajo impacto. Buscar soluciones rápidas o workarounds temporales.", (byte)3, (byte)3, "Medio", 480, 60, (byte)1 },
                    { 7, "#15803D", "success", "Impacto Medio + Urgencia Baja", "BAJA PRIORIDAD - Gestionar en backlog normal. Puede agruparse con otras tareas similares para eficiencia.", (byte)2, (byte)4, "Bajo", 1440, 120, (byte)3 },
                    { 8, "#15803D", "success", "Impacto Bajo + Urgencia Media", "BAJA PRIORIDAD - Resolver cuando sea posible. Mantener comunicación básica con usuario.", (byte)3, (byte)4, "Bajo", 1440, 120, (byte)2 },
                    { 9, "#15803D", "success", "Impacto Bajo + Urgencia Baja", "BAJA PRIORIDAD - Mínimo impacto en el valor. Puede postergarse si surgen prioridades mayores.", (byte)3, (byte)4, "Bajo", 1440, 120, (byte)3 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RolId", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Acceso total al sistema", "Administrador" },
                    { 2, "Gestiona y resuelve incidencias", "Técnico" },
                    { 3, "Registra y da seguimiento a sus tickets", "Solicitante" }
                });

            migrationBuilder.InsertData(
                table: "Sedes",
                columns: new[] { "SedeId", "Nombre", "SedeIdExterno", "TipoSede" },
                values: new object[,]
                {
                    { 1, "DAMASCO", 68, "SALA" },
                    { 2, "EXCALIBUR", 36, "SALA" }
                });

            migrationBuilder.InsertData(
                table: "AcuerdosNivelServicio",
                columns: new[] { "SlaId", "Activo", "CategoriaId", "PrioridadId", "TiempoResolucionMin", "TiempoRespuestaMin" },
                values: new object[,]
                {
                    { 1, true, 1, 1, 60, 15 },
                    { 2, true, 1, 2, 240, 30 },
                    { 3, true, 1, 3, 480, 60 },
                    { 4, true, 2, 1, 60, 15 },
                    { 5, true, 2, 2, 240, 30 },
                    { 6, true, 2, 3, 480, 60 },
                    { 7, true, 2, 4, 1440, 120 },
                    { 8, true, 3, 1, 60, 15 },
                    { 9, true, 3, 2, 240, 30 },
                    { 10, true, 4, 1, 30, 15 },
                    { 11, true, 4, 2, 120, 20 },
                    { 12, true, 7, 3, 480, 60 },
                    { 13, true, 7, 4, 1440, 120 }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "UsuarioId", "Activo", "Apellidos", "Email", "FechaCreacion", "HasFullAccess", "Nombre", "PasswordHash", "PublicId", "RolId", "UserName" },
                values: new object[] { 1, true, "Canchari", "diego.canchari@designdevsoftware.com", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Diego", "$2a$11$2oABfgbGT3nu0gKBQ1C4h.uncd85k9GNxndr8ehlu.yGmtkMhBFse", new Guid("8d04dce2-969a-435d-bba4-df3f325983dc"), 1, "d.cancharir" });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "UsuarioId", "Activo", "Apellidos", "Email", "FechaCreacion", "Nombre", "PasswordHash", "PublicId", "RolId", "UserName" },
                values: new object[,]
                {
                    { 2, true, "Perez", "diego.canchari@designdevsoftware.com", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Juan", "$2a$11$2oABfgbGT3nu0gKBQ1C4h.uncd85k9GNxndr8ehlu.yGmtkMhBFse", new Guid("716578a3-4371-4894-a63d-435e4c0cd3b8"), 1, "administrador" },
                    { 3, true, "Fernandez", "diego.canchari@designdevsoftware.com", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Carlos", "$2a$11$2oABfgbGT3nu0gKBQ1C4h.uncd85k9GNxndr8ehlu.yGmtkMhBFse", new Guid("507da209-c3a9-45e3-aaff-3cf7d334b125"), 1, "tecnico" },
                    { 4, true, "Lopez", "diego.canchari@designdevsoftware.com", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mario", "$2a$11$2oABfgbGT3nu0gKBQ1C4h.uncd85k9GNxndr8ehlu.yGmtkMhBFse", new Guid("4cfa9c79-62b8-49cd-aef7-70f593511d6d"), 1, "solicitante" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcuerdosNivelServicio_CategoriaId_PrioridadId",
                table: "AcuerdosNivelServicio",
                columns: new[] { "CategoriaId", "PrioridadId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AcuerdosNivelServicio_PrioridadId",
                table: "AcuerdosNivelServicio",
                column: "PrioridadId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseConocimiento_CategoriaId",
                table: "BaseConocimiento",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseConocimiento_CreadoPorId",
                table: "BaseConocimiento",
                column: "CreadoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseConocimiento_PublicId",
                table: "BaseConocimiento",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nombre",
                table: "Categorias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComentariosIncidencia_IncidenciaId",
                table: "ComentariosIncidencia",
                column: "IncidenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_ComentariosIncidencia_UsuarioId",
                table: "ComentariosIncidencia",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_EstadosIncidencia_Nombre",
                table: "EstadosIncidencia",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HistorialIncidencias_IncidenciaId",
                table: "HistorialIncidencias",
                column: "IncidenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_HistorialIncidencias_UsuarioId",
                table: "HistorialIncidencias",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidenciaAdjuntos_IncidenciaId",
                table: "IncidenciaAdjuntos",
                column: "IncidenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_CategoriaId",
                table: "Incidencias",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_CerradoPorId",
                table: "Incidencias",
                column: "CerradoPorId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_EscaladoAId",
                table: "Incidencias",
                column: "EscaladoAId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_EstadoId",
                table: "Incidencias",
                column: "EstadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_Fechas",
                table: "Incidencias",
                columns: new[] { "FechaRegistro", "FechaResolucion", "FechaCierre" });

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_NumeroTicket",
                table: "Incidencias",
                column: "NumeroTicket",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_PrioridadId",
                table: "Incidencias",
                column: "PrioridadId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_PublicId",
                table: "Incidencias",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_SedeId",
                table: "Incidencias",
                column: "SedeId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_SolicitanteId",
                table: "Incidencias",
                column: "SolicitanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_TecnicoAsignadoId",
                table: "Incidencias",
                column: "TecnicoAsignadoId");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosRol_PermisoId",
                table: "PermisosRol",
                column: "PermisoId");

            migrationBuilder.CreateIndex(
                name: "IX_PermisosRol_RolId",
                table: "PermisosRol",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Nombre",
                table: "Roles",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_PublicId",
                table: "Usuarios",
                column: "PublicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_RolId",
                table: "Usuarios",
                column: "RolId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_UserName",
                table: "Usuarios",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioSede_SedeId",
                table: "UsuarioSede",
                column: "SedeId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioSede_UsuarioId",
                table: "UsuarioSede",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcuerdosNivelServicio");

            migrationBuilder.DropTable(
                name: "BaseConocimiento");

            migrationBuilder.DropTable(
                name: "ComentariosIncidencia");

            migrationBuilder.DropTable(
                name: "HistorialIncidencias");

            migrationBuilder.DropTable(
                name: "IncidenciaAdjuntos");

            migrationBuilder.DropTable(
                name: "PermisosRol");

            migrationBuilder.DropTable(
                name: "UsuarioSede");

            migrationBuilder.DropTable(
                name: "Incidencias");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "EstadosIncidencia");

            migrationBuilder.DropTable(
                name: "NivelesPrioridad");

            migrationBuilder.DropTable(
                name: "Sedes");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
