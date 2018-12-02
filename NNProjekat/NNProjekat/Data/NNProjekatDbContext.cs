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
        public DbSet<Aktivnost> Aktivnosti { get; set; }
        public DbSet<TipAktivnosti> TipoviAktivnosti { get; set; }
        public DbSet<Predmet> Predmeti { get; set; }
        public DbSet<Student> Studenti { get; set; }
        public DbSet<Osoba> Osobe { get; set; }
        public DbSet<Slusa> Slusanja { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Aktivnost>().HasKey(t => new { t.StudentJMBG, t.NastavnikJMBG, t.Datum, t.SifraTipaAktivnosti, t.SifraPredmeta });
            builder.Entity<TipAktivnosti>().HasKey(t => new { t.SifraTipaAktivnosti, t.SifraPredmeta });
            builder.Entity<Osoba>().HasDiscriminator<string>("tip_osobe").HasValue<Student>("student").HasValue<Nastavnik>("nastavnik");
            builder.Entity<Osoba>().HasKey(t => new { t.JMBG });
            builder.Entity<Predmet>().HasKey(t => new { t.SifraPredmeta });
            builder.Entity<Slusa>().HasKey(t => new { t.SifraPredmeta, t.JMBG});
            builder.Entity<Aktivnost>().HasOne(t => t.Nastavnik).WithMany(t => t.Aktivnosti).HasForeignKey(t => t.NastavnikJMBG).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Aktivnost>().HasOne(t => t.Student).WithMany(t => t.Aktivnosti).HasForeignKey(t => t.StudentJMBG).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
