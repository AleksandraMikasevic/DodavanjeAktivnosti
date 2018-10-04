using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NNProjekat.Data;
using NNProjekat.Models;

namespace NNProjekat.Services
{
    public class SqlPolaganjaData : IPolaganjaData
    {
        private NNProjekatDbContext _context;

        public SqlPolaganjaData(NNProjekatDbContext context)
        {
            _context = context;
        }
        public Polagao Dodaj(Polagao polagao)
        {
            _context.Add(polagao);
            _context.SaveChanges();
            return polagao;
        }

        public Polagao Izmeni(Polagao polagao)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Polagao> UcitajSvePoStudentuIPredmetu(string brojIndeksa, string sifraPredmeta)
        {
            return _context.Polaganja.Include(p => p.Aktivnost)
                .Include(p => p.Nastavnik).Include(p => p.Student).Include(p => p.Aktivnost)
                .Where(p => p.Student.BrojIndeksa == brojIndeksa).Where(p => p.SifraPredmeta == sifraPredmeta).OrderBy(r => r.SifraPredmeta);
        }

        public IEnumerable<Polagao> Vrati(string sifraPredmeta, string brojIndeksa)
        {
            return _context.Polaganja.Include(p => p.Aktivnost)
                .Include(p => p.Nastavnik).Include(p => p.Student).Include(p => p.Aktivnost)
                .Where(p => p.Student.BrojIndeksa == brojIndeksa).Where(p => p.SifraPredmeta == sifraPredmeta).OrderBy(r => r.SifraPredmeta);
        }
    }
}
