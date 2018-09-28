using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.Models
{
    public class Ocena
    {
        public string SifraPredmeta { get; set; }
        public string BrojIndeksa { get; set; }
        public DateTime DatumZakljucivanja { get; set; }
        public int PredlozenaOcena { get; set; }
        public int ZakljucenaOcena { get; set; }
        public Predmet Predmet { get; set; }
        public Student Student { get; set; }
    }
}
