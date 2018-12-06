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
            Console.WriteLine(id + " id kad cuva studentaaaaa");
            Predmet predmet = _context.Predmeti.Where(s => s.SifraPredmeta == id).Single();
            _context.Predmeti.Remove(predmet);
            _context.SaveChanges();
        }

        public void Izmeni(Predmet model)
        {
             var existingParent = _context.Predmeti
                  .Where(p => p.SifraPredmeta == model.SifraPredmeta)
                  .Include(p => p.TipoviAktivnosti)
                  .SingleOrDefault();

              if (existingParent != null)
              {
                  // Update parent
                  _context.Entry(existingParent).CurrentValues.SetValues(model);

                  // Delete children
                  foreach (var existingChild in existingParent.TipoviAktivnosti.ToList())
                  {
                      if (!model.TipoviAktivnosti.Any(c => c.SifraTipaAktivnosti == existingChild.SifraTipaAktivnosti))
                          _context.TipoviAktivnosti.Remove(existingChild);
                  }

                  // Update and Insert children
                  foreach (var childModel in model.TipoviAktivnosti)
                  {
                      var existingChild = existingParent.TipoviAktivnosti
                          .Where(c => c.SifraTipaAktivnosti == childModel.SifraTipaAktivnosti)
                          .SingleOrDefault();

                      if (existingChild != null)
                          // Update child
                          _context.Entry(existingChild).CurrentValues.SetValues(childModel);
                      else
                      {
                        Console.WriteLine("Child");
                          // Insert child
                          var newChild = new TipAktivnosti
                          {
                              SifraTipaAktivnosti = childModel.SifraTipaAktivnosti,
                              Naziv = childModel.Naziv,
                              MinBrojPoena = childModel.MinBrojPoena,
                              MaxBrojPoena = childModel.MaxBrojPoena,
                              TezinskiKoeficijent = childModel.TezinskiKoeficijent,
                              Obavezna = childModel.Obavezna,
                              SifraPredmeta = childModel.SifraPredmeta
                          };
                        _context.TipoviAktivnosti.Add(newChild);
                        Console.WriteLine("Child");
                    }
                }

                  _context.SaveChanges();
              }
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
