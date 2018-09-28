using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NNProjekat.Data;
using NNProjekat.Models;

namespace NNProjekat.Services
{
    public class SqlPredmetData : IPredmetData
    {
        private NNProjekatDbContext _context;

        public SqlPredmetData(NNProjekatDbContext context)
        {
            _context = context;
        }

        public Predmet Dodaj(Predmet predmet)
        {
            _context.Add(predmet);
            _context.SaveChanges();
            return predmet;
        }

        public IQueryable<Predmet> UcitajSve()
        {
            return _context.Predmeti.OrderBy(r => r.Naziv);
        }

        public Predmet Vrati(string sifraPredmeta)
        {
            return _context.Predmeti.Include(p => p.Aktivnosti).FirstOrDefault(r => r.SifraPredmeta == sifraPredmeta);
        }
    }
}
