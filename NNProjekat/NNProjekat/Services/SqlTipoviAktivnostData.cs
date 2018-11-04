using Microsoft.EntityFrameworkCore;
using NNProjekat.Data;
using NNProjekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.Services
{
    public class SqlTipAktivnostiData : ITipAktivnostiData
    {
        private NNProjekatDbContext _context;

        public SqlTipAktivnostiData(NNProjekatDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TipAktivnosti> UcitajSve()
        {
            return _context.TipoviAktivnosti.Include(a => a.Predmet);

        }

        public IEnumerable<TipAktivnosti> UcitajSvePoPredmetu(string sifraPredmeta)
        {
            return _context.TipoviAktivnosti.Include(a => a.Predmet).Where(a=> a.SifraPredmeta == sifraPredmeta).OrderBy(r => r.SifraPredmeta);
        }


    }
}
