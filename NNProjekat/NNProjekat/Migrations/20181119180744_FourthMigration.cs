using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NNProjekat.Migrations
{
    public partial class FourthMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Aktivnosti",
                table: "Aktivnosti");

            migrationBuilder.AlterColumn<string>(
                name: "NastavnikJMBG",
                table: "Aktivnosti",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Aktivnosti",
                table: "Aktivnosti",
                columns: new[] { "StudentJMBG", "Datum", "SifraTipaAktivnosti", "SifraPredmeta" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Aktivnosti",
                table: "Aktivnosti");

            migrationBuilder.AlterColumn<string>(
                name: "NastavnikJMBG",
                table: "Aktivnosti",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Aktivnosti",
                table: "Aktivnosti",
                columns: new[] { "StudentJMBG", "NastavnikJMBG", "Datum", "SifraTipaAktivnosti", "SifraPredmeta" });
        }
    }
}
