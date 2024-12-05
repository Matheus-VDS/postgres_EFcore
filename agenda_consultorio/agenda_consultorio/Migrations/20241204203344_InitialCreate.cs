using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace agenda_consultorio.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    CPF = table.Column<string>(type: "text", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Idade = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.CPF);
                });

            migrationBuilder.CreateTable(
                name: "Consultas",
                columns: table => new
                {
                    CPF = table.Column<string>(type: "text", nullable: false),
                    DataConsulta = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HoraInicial = table.Column<TimeSpan>(type: "interval", nullable: false),
                    HoraFinal = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consultas", x => new { x.CPF, x.DataConsulta, x.HoraInicial });
                    table.ForeignKey(
                        name: "FK_Consultas_Pacientes_CPF",
                        column: x => x.CPF,
                        principalTable: "Pacientes",
                        principalColumn: "CPF",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Consultas");

            migrationBuilder.DropTable(
                name: "Pacientes");
        }
    }
}
