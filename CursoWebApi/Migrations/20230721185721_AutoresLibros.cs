using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CursoWebApi.Migrations
{
    public partial class AutoresLibros : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AutoresLibros",
                columns: table => new
                {
                    Libroid = table.Column<int>(type: "int", nullable: false),
                    Autorid = table.Column<int>(type: "int", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoresLibros", x => new { x.Autorid, x.Libroid });
                    table.ForeignKey(
                        name: "FK_AutoresLibros_Autores_Autorid",
                        column: x => x.Autorid,
                        principalTable: "Autores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AutoresLibros_Libros_Libroid",
                        column: x => x.Libroid,
                        principalTable: "Libros",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutoresLibros_Libroid",
                table: "AutoresLibros",
                column: "Libroid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoresLibros");
        }
    }
}
