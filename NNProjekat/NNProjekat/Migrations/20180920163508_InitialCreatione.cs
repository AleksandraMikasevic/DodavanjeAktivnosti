using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace NNProjekat.Migrations
{
    public partial class InitialCreatione : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nastavnici",
                columns: table => new
                {
                    JMBG = table.Column<string>(nullable: false),
                    Ime = table.Column<string>(nullable: true),
                    Pozicija = table.Column<string>(nullable: true),
                    Prezime = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nastavnici", x => x.JMBG);
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
                name: "Studenti",
                columns: table => new
                {
                    BrojIndeksa = table.Column<string>(nullable: false),
                    Ime = table.Column<string>(nullable: true),
                    Prezime = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Studenti", x => x.BrojIndeksa);
                });

            migrationBuilder.CreateTable(
                name: "Aktivnosti",
                columns: table => new
                {
                    SifraAktivnosti = table.Column<string>(nullable: false),
                    SifraPredmeta = table.Column<string>(nullable: false),
                    MaxBrojPoena = table.Column<double>(nullable: false),
                    MinBrojPoena = table.Column<double>(nullable: false),
                    Naziv = table.Column<string>(nullable: true),
                    Obavezna = table.Column<bool>(nullable: false),
                    TezinskiKoeficijent = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aktivnosti", x => new { x.SifraAktivnosti, x.SifraPredmeta });
                    table.ForeignKey(
                        name: "FK_Aktivnosti_Predmeti_SifraPredmeta",
                        column: x => x.SifraPredmeta,
                        principalTable: "Predmeti",
                        principalColumn: "SifraPredmeta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ocene",
                columns: table => new
                {
                    BrojIndeksa = table.Column<string>(nullable: false),
                    SifraPredmeta = table.Column<string>(nullable: false),
                    DatumZakljucivanja = table.Column<DateTime>(nullable: false),
                    PredlozenaOcena = table.Column<int>(nullable: false),
                    ZakljucenaOcena = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ocene", x => new { x.BrojIndeksa, x.SifraPredmeta, x.DatumZakljucivanja });
                    table.ForeignKey(
                        name: "FK_Ocene_Studenti_BrojIndeksa",
                        column: x => x.BrojIndeksa,
                        principalTable: "Studenti",
                        principalColumn: "BrojIndeksa",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Ocene_Predmeti_SifraPredmeta",
                        column: x => x.SifraPredmeta,
                        principalTable: "Predmeti",
                        principalColumn: "SifraPredmeta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Polaganja",
                columns: table => new
                {
                    BrojIndeksa = table.Column<string>(nullable: false),
                    JMBG = table.Column<string>(nullable: false),
                    Datum = table.Column<DateTime>(nullable: false),
                    SifraAktivnosti = table.Column<string>(nullable: false),
                    SifraPredmeta = table.Column<string>(nullable: false),
                    BrojPoena = table.Column<double>(nullable: false),
                    Status = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polaganja", x => new { x.BrojIndeksa, x.JMBG, x.Datum, x.SifraAktivnosti, x.SifraPredmeta });
                    table.ForeignKey(
                        name: "FK_Polaganja_Studenti_BrojIndeksa",
                        column: x => x.BrojIndeksa,
                        principalTable: "Studenti",
                        principalColumn: "BrojIndeksa",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Polaganja_Nastavnici_JMBG",
                        column: x => x.JMBG,
                        principalTable: "Nastavnici",
                        principalColumn: "JMBG",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Polaganja_Aktivnosti_SifraAktivnosti_SifraPredmeta",
                        columns: x => new { x.SifraAktivnosti, x.SifraPredmeta },
                        principalTable: "Aktivnosti",
                        principalColumns: new[] { "SifraAktivnosti", "SifraPredmeta" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Aktivnosti_SifraPredmeta",
                table: "Aktivnosti",
                column: "SifraPredmeta");

            migrationBuilder.CreateIndex(
                name: "IX_Ocene_SifraPredmeta",
                table: "Ocene",
                column: "SifraPredmeta");

            migrationBuilder.CreateIndex(
                name: "IX_Polaganja_JMBG",
                table: "Polaganja",
                column: "JMBG");

            migrationBuilder.CreateIndex(
                name: "IX_Polaganja_SifraAktivnosti_SifraPredmeta",
                table: "Polaganja",
                columns: new[] { "SifraAktivnosti", "SifraPredmeta" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ocene");

            migrationBuilder.DropTable(
                name: "Polaganja");

            migrationBuilder.DropTable(
                name: "Studenti");

            migrationBuilder.DropTable(
                name: "Nastavnici");

            migrationBuilder.DropTable(
                name: "Aktivnosti");

            migrationBuilder.DropTable(
                name: "Predmeti");
        }
    }
}
