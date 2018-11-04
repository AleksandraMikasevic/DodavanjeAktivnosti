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

        public Aktivnost Izmeni(Aktivnost aktivnost)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Aktivnost> UcitajSve()
        {
            return _context.Aktivnosti.Include(p => p.TipAktivnosti).Include(p => p.Student).Include(p => p.Nastavnik).Include(p => p.TipAktivnosti.Predmet);
        }

        public IEnumerable<Aktivnost> UcitajSvePoStudentuIPredmetu(string JMBGS, string sifraPredmeta)
        {
            return _context.Aktivnosti.Include(p => p.TipAktivnosti)
                .Include(p => p.Nastavnik).Include(p => p.Student).Include(p => p.TipAktivnosti)
                .Where(p => p.Student.JMBG== JMBGS).Where(p => p.SifraPredmeta == sifraPredmeta).OrderBy(r => r.SifraPredmeta);
        }

        public IEnumerable<Aktivnost> Vrati(string sifraPredmeta, string JMBGS)
        {
            return _context.Aktivnosti.Include(p => p.TipAktivnosti)
                .Include(p => p.Nastavnik).Include(p => p.Student).Include(p => p.TipAktivnosti)
                .Where(p => p.Student.JMBG == JMBGS).Where(p => p.SifraPredmeta == sifraPredmeta).OrderBy(r => r.SifraPredmeta);
        }
    }
}
