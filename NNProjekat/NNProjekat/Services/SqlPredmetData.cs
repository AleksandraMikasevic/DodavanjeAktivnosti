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

        public void Izbrisi(string id)
        {
            Console.WriteLine(id+" id kad cuva studentaaaaa");
            Predmet predmet = _context.Predmeti.Where(s => s.SifraPredmeta == id).Single();
            _context.Predmeti.Remove(predmet);
            _context.SaveChanges();
        }

        public void Izmeni(Predmet predmet)
        {
            _context.Predmeti.Update(predmet);
            _context.SaveChanges();
        }

        public IQueryable<Predmet> UcitajSve()
        {
            return _context.Predmeti.OrderBy(r => r.Naziv);
        }

        public Predmet Vrati(string sifraPredmeta)
        {
            return _context.Predmeti.Include(p => p.TipoviAktivnosti).FirstOrDefault(r => r.SifraPredmeta == sifraPredmeta);
        }

        public IEnumerable<TipAktivnosti> VratiIzmeni(string sifraPredmeta)
        {
            return _context.TipoviAktivnosti.Where(a => a.SifraPredmeta == sifraPredmeta);

        }

       
    }
}
