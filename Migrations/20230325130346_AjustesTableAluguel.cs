using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAluguelCavalos.Migrations
{
    public partial class AjustesTableAluguel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClienteId1",
                table: "Alugueis",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alugueis_ClienteId1",
                table: "Alugueis",
                column: "ClienteId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Alugueis_AspNetUsers_ClienteId1",
                table: "Alugueis",
                column: "ClienteId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alugueis_AspNetUsers_ClienteId1",
                table: "Alugueis");

            migrationBuilder.DropIndex(
                name: "IX_Alugueis_ClienteId1",
                table: "Alugueis");

            migrationBuilder.DropColumn(
                name: "ClienteId1",
                table: "Alugueis");
        }
    }
}
