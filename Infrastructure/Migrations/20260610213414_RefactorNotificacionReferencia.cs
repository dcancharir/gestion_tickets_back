using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorNotificacionReferencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumeroTicket",
                table: "Notificaciones");

            migrationBuilder.DropColumn(
                name: "TicketPublicId",
                table: "Notificaciones");

            migrationBuilder.AddColumn<string>(
                name: "Referencia",
                table: "Notificaciones",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Referencia",
                table: "Notificaciones");

            migrationBuilder.AddColumn<string>(
                name: "NumeroTicket",
                table: "Notificaciones",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TicketPublicId",
                table: "Notificaciones",
                type: "nvarchar(36)",
                maxLength: 36,
                nullable: true);
        }
    }
}
