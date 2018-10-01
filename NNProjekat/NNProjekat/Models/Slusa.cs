using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.Models
{
    public class Slusa
    {
        public DateTime DatumPrvogUpisa { get; set; }
        public string BrojIndeksa { get; set; }
        public string SifraPredmeta { get; set; }
        public Student Student { get; set; }
        public Predmet Predmet { get; set; }
    }
}
