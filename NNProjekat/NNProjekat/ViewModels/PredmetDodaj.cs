using NNProjekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.ViewModels
{
    public class PredmetDodaj
    {
        public string SifraPredmeta { get; set; }
        public string Naziv { get; set; }
        public int BrojESPB { get; set; }
        public List<TipAktivnosti> TipoviAktivnosti { get; set; }

        public PredmetDodaj()
        {
            TipoviAktivnosti = new List<TipAktivnosti>();
        }
    }
}
