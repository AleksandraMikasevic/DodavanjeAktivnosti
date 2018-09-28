using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.Models
{
    public class Predmet
    {
        public string SifraPredmeta { get; set; }
        public string Naziv { get; set; }
        public int BrojESPB { get; set; }
        public IEnumerable<Aktivnost> Aktivnosti { get; set; }
    }
}
