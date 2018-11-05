using NNProjekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.Services
{
    public interface IAktivnostiData
    {
        IEnumerable<Aktivnost> UcitajSvePoStudentuIPredmetu(string brojIndeksa, string sifraPredmeta);
        Aktivnost Dodaj(Aktivnost aktivnost);
        Aktivnost Izmeni(Aktivnost aktivnost);
        Aktivnost Izbrisi(Aktivnost aktivnost);
        IEnumerable<Aktivnost> Vrati(string sifraPredmeta, string brojIndeksa);
        IEnumerable<Aktivnost> UcitajSve();
        IEnumerable<Aktivnost> Ucitaj(string studentJMBG, string sifraPredmeta, string sifraTipaAktivnosti);
        IEnumerable<Aktivnost> UcitajSvePoStudentuIPredmetuPrikaz(string jMBG, string sifraPredmeta);
        IEnumerable<Aktivnost> UcitajSveValidne();
    }
}
