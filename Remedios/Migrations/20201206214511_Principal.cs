using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Remedios.Migrations
{
    public partial class Principal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MembrosFamilia",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Sobrenome = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    CPF = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    Nascimento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembrosFamilia", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Receitas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Diagnostico = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Medico = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Instrucao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Temporario = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receitas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Remedios",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Preco = table.Column<double>(type: "float", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    Tarja = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Validade = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remedios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fotos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    foto = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MembroId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fotos_MembrosFamilia_MembroId",
                        column: x => x.MembroId,
                        principalTable: "MembrosFamilia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MembroRemedios",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RemedioId = table.Column<long>(type: "bigint", nullable: false),
                    ReceitaId = table.Column<long>(type: "bigint", nullable: true),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembroRemedios", x => new { x.RemedioId, x.UserId });
                    table.ForeignKey(
                        name: "FK_MembroRemedios_MembrosFamilia_UserId",
                        column: x => x.UserId,
                        principalTable: "MembrosFamilia",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MembroRemedios_Receitas_ReceitaId",
                        column: x => x.ReceitaId,
                        principalTable: "Receitas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MembroRemedios_Remedios_RemedioId",
                        column: x => x.RemedioId,
                        principalTable: "Remedios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Doses",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataUso = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MembroRemedioRemedioId = table.Column<long>(type: "bigint", nullable: true),
                    MembroRemedioUserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Doses_MembroRemedios_MembroRemedioRemedioId_MembroRemedioUserId",
                        columns: x => new { x.MembroRemedioRemedioId, x.MembroRemedioUserId },
                        principalTable: "MembroRemedios",
                        principalColumns: new[] { "RemedioId", "UserId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Doses_MembroRemedioRemedioId_MembroRemedioUserId",
                table: "Doses",
                columns: new[] { "MembroRemedioRemedioId", "MembroRemedioUserId" });

            migrationBuilder.CreateIndex(
                name: "IX_Fotos_MembroId",
                table: "Fotos",
                column: "MembroId");

            migrationBuilder.CreateIndex(
                name: "IX_MembroRemedios_ReceitaId",
                table: "MembroRemedios",
                column: "ReceitaId");

            migrationBuilder.CreateIndex(
                name: "IX_MembroRemedios_UserId",
                table: "MembroRemedios",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Doses");

            migrationBuilder.DropTable(
                name: "Fotos");

            migrationBuilder.DropTable(
                name: "MembroRemedios");

            migrationBuilder.DropTable(
                name: "MembrosFamilia");

            migrationBuilder.DropTable(
                name: "Receitas");

            migrationBuilder.DropTable(
                name: "Remedios");
        }
    }
}
