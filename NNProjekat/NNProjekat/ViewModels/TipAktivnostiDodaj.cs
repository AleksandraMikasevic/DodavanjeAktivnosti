using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.ViewModels
{
    public class TipAktivnostiDodaj
    {
        public string Naziv { get; set; }
        public string SifraTipaAktivnosti { get; set; }
        public double MinBrojPoena { get; set; }
        public double MaxBrojPoena { get; set; }
        public double TezinskiKoeficijent { get; set; }
        public bool Obavezna { get; set; }
    }
}
