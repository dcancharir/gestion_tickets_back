using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class sedeyadjuntos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SedeId",
                table: "Incidencias",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.InsertData(
                table: "Sedes",
                columns: new[] { "SedeId", "Nombre", "SedeIdExterno", "TipoSede" },
                values: new object[,]
                {
                    { 1, "DAMASCO", 68, "SALA" },
                    { 2, "EXCALIBUR", 36, "SALA" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_SedeId",
                table: "Incidencias",
                column: "SedeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidenciaAdjuntos_IncidenciaId",
                table: "IncidenciaAdjuntos",
                column: "IncidenciaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidencias_Sedes_SedeId",
                table: "Incidencias",
                column: "SedeId",
                principalTable: "Sedes",
                principalColumn: "SedeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidencias_Sedes_SedeId",
                table: "Incidencias");

            migrationBuilder.DropTable(
                name: "IncidenciaAdjuntos");

            migrationBuilder.DropTable(
                name: "Sedes");

            migrationBuilder.DropIndex(
                name: "IX_Incidencias_SedeId",
                table: "Incidencias");

            migrationBuilder.DropColumn(
                name: "SedeId",
                table: "Incidencias");
        }
    }
}
