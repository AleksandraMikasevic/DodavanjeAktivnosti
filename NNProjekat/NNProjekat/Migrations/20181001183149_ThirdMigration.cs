using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NNProjekat.Migrations
{
    public partial class ThirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Slusanja",
                columns: table => new
                {
                    SifraPredmeta = table.Column<string>(nullable: false),
                    BrojIndeksa = table.Column<string>(nullable: false),
                    DatumPrvogUpisa = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slusanja", x => new { x.SifraPredmeta, x.BrojIndeksa });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Slusanja");
        }
    }
}
