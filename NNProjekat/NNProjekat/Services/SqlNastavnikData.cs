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
        public IEnumerable<Nastavnik> UcitajSve()
        {
            return _context.Nastavnici.OrderBy(n => n.Prezime);
        }

        public Nastavnik Vrati(string jMBG)
        {
            return _context.Nastavnici.FirstOrDefault(n => n.JMBG == jMBG);
        }
    }
}
