using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NNProjekat.Migrations
{
    public partial class SecondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Osobe",
                columns: table => new
                {
                    Pozicija = table.Column<string>(nullable: true),
                    JMBG = table.Column<string>(nullable: false),
                    Ime = table.Column<string>(nullable: true),
                    Prezime = table.Column<string>(nullable: true),
                    tip_osobe = table.Column<string>(nullable: false),
                    BrojIndeksa = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Osobe", x => x.JMBG);
                });

            migrationBuilder.CreateTable(
                name: "Predmeti",
                columns: table => new
                {
                    SifraPredmeta = table.Column<string>(nullable: false),
                    BrojESPB = table.Column<int>(nullable: false),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predmeti", x => x.SifraPredmeta);
                });

            migrationBuilder.CreateTable(
                name: "Slusanja",
                columns: table => new
                {
                    SifraPredmeta = table.Column<string>(nullable: false),
                    JMBG = table.Column<string>(nullable: false),
                    DatumPrvogUpisa = table.Column<DateTime>(nullable: false),
                    DatumZakljucivanja = table.Column<DateTime>(nullable: false),
                    PredlozenaOcena = table.Column<int>(nullable: false),
                    ZakljucenaOcena = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slusanja", x => new { x.SifraPredmeta, x.JMBG });
                    table.ForeignKey(
                        name: "FK_Slusanja_Osobe_JMBG",
                        column: x => x.JMBG,
                        principalTable: "Osobe",
                        principalColumn: "JMBG",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Slusanja_Predmeti_SifraPredmeta",
                        column: x => x.SifraPredmeta,
                        principalTable: "Predmeti",
                        principalColumn: "SifraPredmeta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TipoviAktivnosti",
                columns: table => new
                {
                    SifraTipaAktivnosti = table.Column<string>(nullable: false),
                    SifraPredmeta = table.Column<string>(nullable: false),
                    MaxBrojPoena = table.Column<double>(nullable: false),
                    MinBrojPoena = table.Column<double>(nullable: false),
                    Naziv = table.Column<string>(nullable: true),
                    Obavezna = table.Column<bool>(nullable: false),
                    TezinskiKoeficijent = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoviAktivnosti", x => new { x.SifraTipaAktivnosti, x.SifraPredmeta });
                    table.ForeignKey(
                        name: "FK_TipoviAktivnosti_Predmeti_SifraPredmeta",
                        column: x => x.SifraPredmeta,
                        principalTable: "Predmeti",
                        principalColumn: "SifraPredmeta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Aktivnosti",
                columns: table => new
                {
                    StudentJMBG = table.Column<string>(nullable: false),
                    NastavnikJMBG = table.Column<string>(nullable: false),
                    Datum = table.Column<DateTime>(nullable: false),
                    SifraTipaAktivnosti = table.Column<string>(nullable: false),
                    SifraPredmeta = table.Column<string>(nullable: false),
                    BrojPoena = table.Column<double>(nullable: false),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aktivnosti", x => new { x.StudentJMBG, x.NastavnikJMBG, x.Datum, x.SifraTipaAktivnosti, x.SifraPredmeta });
                    table.ForeignKey(
                        name: "FK_Aktivnosti_Osobe_NastavnikJMBG",
                        column: x => x.NastavnikJMBG,
                        principalTable: "Osobe",
                        principalColumn: "JMBG",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Aktivnosti_Osobe_StudentJMBG",
                        column: x => x.StudentJMBG,
                        principalTable: "Osobe",
                        principalColumn: "JMBG",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Aktivnosti_TipoviAktivnosti_SifraTipaAktivnosti_SifraPredmeta",
                        columns: x => new { x.SifraTipaAktivnosti, x.SifraPredmeta },
                        principalTable: "TipoviAktivnosti",
                        principalColumns: new[] { "SifraTipaAktivnosti", "SifraPredmeta" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aktivnosti_NastavnikJMBG",
                table: "Aktivnosti",
                column: "NastavnikJMBG");

            migrationBuilder.CreateIndex(
                name: "IX_Aktivnosti_SifraTipaAktivnosti_SifraPredmeta",
                table: "Aktivnosti",
                columns: new[] { "SifraTipaAktivnosti", "SifraPredmeta" });

            migrationBuilder.CreateIndex(
                name: "IX_Slusanja_JMBG",
                table: "Slusanja",
                column: "JMBG");

            migrationBuilder.CreateIndex(
                name: "IX_TipoviAktivnosti_SifraPredmeta",
                table: "TipoviAktivnosti",
                column: "SifraPredmeta");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aktivnosti");

            migrationBuilder.DropTable(
                name: "Slusanja");

            migrationBuilder.DropTable(
                name: "TipoviAktivnosti");

            migrationBuilder.DropTable(
                name: "Osobe");

            migrationBuilder.DropTable(
                name: "Predmeti");
        }
    }
}
