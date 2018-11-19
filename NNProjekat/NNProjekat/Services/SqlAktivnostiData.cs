using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NNProjekat.Data;
using NNProjekat.Models;

namespace NNProjekat.Services
{
    public class SqlAktivnostiData : IAktivnostiData
    {
        private NNProjekatDbContext _context;

        public SqlAktivnostiData(NNProjekatDbContext context)
        {
            _context = context;
        }
        public Aktivnost Dodaj(Aktivnost aktivnost)
        {
            _context.Add(aktivnost);
            _context.SaveChanges();
            return aktivnost;
        }

        public Aktivnost Izbrisi(Aktivnost aktivnost)
        {
            _context.Aktivnosti.Update(aktivnost);
            _context.SaveChanges();
            return aktivnost;
        }

        public Aktivnost Izmeni(Aktivnost aktivnost)
        {
            _context.Aktivnosti.Update(aktivnost);
            _context.SaveChanges();
            return aktivnost;
        }

        public IEnumerable<Aktivnost> Ucitaj(string studentJMBG, string sifraPredmeta, string sifraTipaAktivnosti)
        {
            return _context.Aktivnosti.Include(p => p.TipAktivnosti)
                .Include(p => p.Nastavnik).Include(p => p.Student).Include(p => p.TipAktivnosti)
                .Where(p => p.Student.JMBG == studentJMBG).Where(p => p.SifraTipaAktivnosti == sifraTipaAktivnosti).Where(p => p.SifraPredmeta == sifraPredmeta).OrderBy(r => r.SifraPredmeta);

        }

        public IEnumerable<Aktivnost> UcitajSve()
        {
            return _context.Aktivnosti.Include(p => p.TipAktivnosti).Include(p => p.Student).Include(p => p.Nastavnik).Include(p => p.TipAktivnosti.Predmet);
        }

        public IEnumerable<Aktivnost> UcitajSvePoStudentuIPredmetu(string JMBGS, string sifraPredmeta)
        {
            return _context.Aktivnosti.Include(p => p.TipAktivnosti)
                .Include(p => p.Nastavnik).Include(p => p.Student).Include(p => p.TipAktivnosti)
                .Where(p => p.Student.JMBG == JMBGS).Where(p => p.SifraPredmeta == sifraPredmeta).OrderBy(r => r.SifraPredmeta);
        }

        public IEnumerable<Aktivnost> UcitajSvePoStudentuIPredmetuPrikaz(string jMBG, string sifraPredmeta)
        {
            return _context.Aktivnosti.Include(p => p.TipAktivnosti)
    .Include(p => p.Nastavnik).Include(p => p.Student).Include(p => p.TipAktivnosti)
    .Where(p => p.Student.JMBG == jMBG).Where(p => p.SifraPredmeta == sifraPredmeta).Where(a => a.Validna == true).OrderBy(r => r.SifraPredmeta);

        }

        public IEnumerable<Aktivnost> UcitajSveValidne()
        {
            return _context.Aktivnosti.Include(p => p.TipAktivnosti).Include(p => p.Student).Include(p => p.Nastavnik).Include(p => p.TipAktivnosti.Predmet).Where(a => a.Validna == true);
        }

        public IEnumerable<Aktivnost> Vrati(string sifraPredmeta, string JMBGS)
        {
            return _context.Aktivnosti.Include(p => p.TipAktivnosti)
                .Include(p => p.Nastavnik).Include(p => p.Student).Include(p => p.TipAktivnosti)
                .Where(p => p.Student.JMBG == JMBGS).Where(p => p.SifraPredmeta == sifraPredmeta).OrderBy(r => r.SifraPredmeta);
        }

        public Aktivnost Vrati(string jMBGS, string sifraPredmeta, string sifraTipaAktivnosti, DateTime datum)
        {
            return _context.Aktivnosti.Include(a => a.Nastavnik).Include(a => a.Student).Include(a => a.TipAktivnosti).Include(a => a.TipAktivnosti.Predmet)
                .Where(a => a.StudentJMBG == jMBGS && a.SifraTipaAktivnosti == sifraTipaAktivnosti && a.SifraPredmeta == sifraPredmeta && a.Datum == datum).FirstOrDefault();
        }
    }
}
