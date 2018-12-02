using NNProjekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.ViewModels
{
    public class StudentPrikazStudenta
    {
        public Slusa Slusa { get; set; }
        public IEnumerable<Aktivnost> AktivnostiStudenta { get; set; }
        public String Nastavnik { get; set; }
        public DateTime Datum { get; set; }
    }
}
