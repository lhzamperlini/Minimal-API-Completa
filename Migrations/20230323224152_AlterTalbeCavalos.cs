using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiAluguelCavalos.Migrations
{
    public partial class AlterTalbeCavalos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_cavalos",
                table: "cavalos");

            migrationBuilder.RenameTable(
                name: "cavalos",
                newName: "Cavalos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Cavalos",
                table: "Cavalos",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Cavalos",
                table: "Cavalos");

            migrationBuilder.RenameTable(
                name: "Cavalos",
                newName: "cavalos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_cavalos",
                table: "cavalos",
                column: "Id");
        }
    }
}
