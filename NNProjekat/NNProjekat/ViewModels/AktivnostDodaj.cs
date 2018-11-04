using Microsoft.AspNetCore.Mvc.Rendering;
using NNProjekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.ViewModels
{
    public class AktivnostDodaj
    {
        public IEnumerable<Nastavnik> Nastavnici { get; set; }
        public IEnumerable<Student> Studenti { get; set; }
        public IEnumerable<Predmet> Predmeti { get; set; }
        public IEnumerable<TipAktivnosti> TipoviAktivnosti { get; set; }
        public string StudentJMBG { get; set; }
        public string NastavnikJMBG { get; set; }
        public string SifraTipaAktivnosti { get; set; }
        public string SifraPredmeta { get; set; }
        public double BrojPoena { get; set; }
        public bool Status { get; set; }
        public DateTime Datum { get; set; }
        public Nastavnik IzabraniNastavnik { get; set; }
        public Student IzabraniStudent { get; set; }
        public TipAktivnosti TipAktivnosti { get; set; }

    }
}
