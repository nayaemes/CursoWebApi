using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CursoWebApi.Migrations
{
    public partial class ComentarioUsuario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Usuarioid",
                table: "Comentarios",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comentarios_Usuarioid",
                table: "Comentarios",
                column: "Usuarioid");

            migrationBuilder.AddForeignKey(
                name: "FK_Comentarios_AspNetUsers_Usuarioid",
                table: "Comentarios",
                column: "Usuarioid",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comentarios_AspNetUsers_Usuarioid",
                table: "Comentarios");

            migrationBuilder.DropIndex(
                name: "IX_Comentarios_Usuarioid",
                table: "Comentarios");

            migrationBuilder.DropColumn(
                name: "Usuarioid",
                table: "Comentarios");
        }
    }
}
