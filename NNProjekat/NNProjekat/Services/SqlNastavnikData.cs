using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NNProjekat.Data;
using NNProjekat.Models;

namespace NNProjekat.Services
{
    public class SqlNastavnikData : INastavnikData
    {
        private NNProjekatDbContext _context;

        public SqlNastavnikData(NNProjekatDbContext context)
        {
            _context = context;
        }

        public bool ProveriNastavnika(string username, string password)
        {
            Nastavnik nastavnik = null;
            try
            {
                nastavnik = _context.Nastavnici.Where(n => n.KorisnickoIme == username & n.Lozinka == password).Single();
            }
            catch (Exception ex) {
                return false;
            }
            if (nastavnik == null)
                return false;
            return true;
        }

        public IEnumerable<Nastavnik> UcitajSve()
        {
            return _context.Nastavnici.OrderBy(n => n.Prezime);
        }

        public Nastavnik Vrati(string jMBG)
        {
            return _context.Nastavnici.FirstOrDefault(n => n.JMBG == jMBG);
        }

        public Nastavnik VratiPoUsername(string username)
        {
            return _context.Nastavnici.FirstOrDefault(n => n.KorisnickoIme == username);

        }
    }
}
