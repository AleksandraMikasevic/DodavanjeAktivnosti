using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NNProjekat.Migrations
{
    public partial class FourthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Slusanja_BrojIndeksa",
                table: "Slusanja",
                column: "BrojIndeksa");

            migrationBuilder.AddForeignKey(
                name: "FK_Slusanja_Studenti_BrojIndeksa",
                table: "Slusanja",
                column: "BrojIndeksa",
                principalTable: "Studenti",
                principalColumn: "BrojIndeksa",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Slusanja_Predmeti_SifraPredmeta",
                table: "Slusanja",
                column: "SifraPredmeta",
                principalTable: "Predmeti",
                principalColumn: "SifraPredmeta",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Slusanja_Studenti_BrojIndeksa",
                table: "Slusanja");

            migrationBuilder.DropForeignKey(
                name: "FK_Slusanja_Predmeti_SifraPredmeta",
                table: "Slusanja");

            migrationBuilder.DropIndex(
                name: "IX_Slusanja_BrojIndeksa",
                table: "Slusanja");
        }
    }
}
