using NNProjekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.ViewModels
{
    public class StudentSviStudenti
    {
        public IEnumerable<Slusa> Studenti { get; set; }
        public string SifraPredmeta { get; set; }
        public Predmet Predmet { get; set; }
        public List<StudentSviStudentiMod> Mod { get; set; }
    }
}
