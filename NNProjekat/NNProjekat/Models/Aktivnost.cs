using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.Models
{
    public class Aktivnost
    {
        public string Naziv { get; set; }
        public string SifraAktivnosti { get; set; }
        public string SifraPredmeta { get; set; }
        public double MinBrojPoena { get; set; }
        public double MaxBrojPoena { get; set; }
        public double TezinskiKoeficijent { get; set; }
        public bool Obavezna { get; set; }
        public Predmet Predmet { get; set; }
    }
}
