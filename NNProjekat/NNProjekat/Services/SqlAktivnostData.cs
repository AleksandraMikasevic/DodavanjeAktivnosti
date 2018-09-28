using Microsoft.EntityFrameworkCore;
using NNProjekat.Data;
using NNProjekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.Services
{
    public class SqlAktivnostData : IAktivnostData
    {
        private NNProjekatDbContext _context;

        public SqlAktivnostData(NNProjekatDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Aktivnost> UcitajSvePoPredmetu(string sifraPredmeta)
        {
            return _context.Aktivnosti.Include(a => a.Predmet).Where(a=> a.SifraPredmeta == sifraPredmeta).OrderBy(r => r.SifraPredmeta);
        }
    }
}
