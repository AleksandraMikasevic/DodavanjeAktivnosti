using Microsoft.EntityFrameworkCore;
using NNProjekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.Data
{
    public class NNProjekatDbContext : DbContext
    {
        public NNProjekatDbContext(DbContextOptions options)
        : base(options)
        {

        }
        public DbSet<Nastavnik> Nastavnici { get; set; }
        public DbSet<Ocena> Ocene { get; set; }
        public DbSet<Aktivnost> Aktivnosti { get; set; }
        public DbSet<Polagao> Polaganja { get; set; }
        public DbSet<Predmet> Predmeti { get; set; }
        public DbSet<Student> Studenti { get; set; }
        public DbSet<Slusa> Slusanja { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Polagao>().HasKey(t => new { t.BrojIndeksa, t.JMBG, t.Datum, t.SifraAktivnosti, t.SifraPredmeta });
            builder.Entity<Aktivnost>().HasKey(t => new { t.SifraAktivnosti, t.SifraPredmeta });
            builder.Entity<Ocena>().HasKey(t => new { t.BrojIndeksa, t.SifraPredmeta, t.DatumZakljucivanja });
            builder.Entity<Nastavnik>().HasKey(t => new { t.JMBG });
            builder.Entity<Student>().HasKey(t => new { t.BrojIndeksa });
            builder.Entity<Predmet>().HasKey(t => new { t.SifraPredmeta });
            builder.Entity<Slusa>().HasKey(t => new { t.SifraPredmeta, t.BrojIndeksa});

        }
    }
}
