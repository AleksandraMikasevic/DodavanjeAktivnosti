using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.Models
{
    public class Polagao
    {
        public DateTime Datum { get; set; }
        public string BrojIndeksa { get; set; }
        public string JMBG { get; set; }
        public string SifraAktivnosti { get; set; }
        public string SifraPredmeta { get; set; }
        public double BrojPoena { get; set; }
        public bool Status { get; set; }
        public Nastavnik Nastavnik { get; set; }
        public Student Student { get; set; }
        public Aktivnost Aktivnost { get; set; }
    }
}
