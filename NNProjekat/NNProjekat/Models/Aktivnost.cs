using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.Models
{
    public class Aktivnost
    {
        public DateTime Datum { get; set; }
        public string NastavnikJMBG { get; set; }
        public string StudentJMBG { get; set; }
        public string SifraTipaAktivnosti { get; set; }
        public string SifraPredmeta { get; set; }
        public double BrojPoena { get; set; }
        public bool Status { get; set; }
        public Nastavnik Nastavnik { get; set; }
        public Student Student { get; set; }
        public TipAktivnosti TipAktivnosti { get; set; }
    }
}
