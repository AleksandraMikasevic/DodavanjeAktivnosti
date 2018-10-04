using Microsoft.AspNetCore.Mvc.Rendering;
using NNProjekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.ViewModels
{
    public class PolagaoDodaj
    {
        public IEnumerable<Nastavnik> Nastavnici { get; set; }
        public IEnumerable<Student> Studenti { get; set; }
        public IEnumerable<Predmet> Predmeti { get; set; }
        public IEnumerable<Aktivnost> Aktivnosti { get; set; }
        public string BrojIndeksa { get; set; }
        public string JMBG { get; set; }
        public string SifraAktivnosti { get; set; }
        public string SifraPredmeta { get; set; }
        public double BrojPoena { get; set; }
        public bool Status { get; set; }
        public DateTime Datum { get; set; }
        public Nastavnik IzabraniNastavnik { get; set; }
        public Student IzabraniStudent { get; set; }
        public Aktivnost Aktivnost { get; set; }

    }
}
