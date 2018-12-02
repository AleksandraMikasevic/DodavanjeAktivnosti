using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NNProjekat.Migrations
{
    public partial class Eight : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KorisnickoIme",
                table: "Osobe",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Lozinka",
                table: "Osobe",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Zapamti",
                table: "Osobe",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KorisnickoIme",
                table: "Osobe");

            migrationBuilder.DropColumn(
                name: "Lozinka",
                table: "Osobe");

            migrationBuilder.DropColumn(
                name: "Zapamti",
                table: "Osobe");
        }
    }
}
