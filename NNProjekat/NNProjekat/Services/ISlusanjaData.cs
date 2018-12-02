using NNProjekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.Services
{
    public interface ISlusanjaData
    {
        IEnumerable<Slusa> UcitajSve();
        IEnumerable<Slusa> UcitajSve(string JMBG);
        IEnumerable<Slusa> UcitajSveStudente(string sifraPredmeta);
        Slusa Vrati(string JMBG, string sifraPredmeta);
        void IzracunajOcenu(string JMBG, string sifraPredmeta, IEnumerable<Aktivnost> aktivnostiStudenta);
        void ZakljuciOcenu(string JMBG, string sifraPredmeta);
        void Sacuvaj(Slusa slusa);
        void Izbrisi(Slusa slusa);
        void ZakljuciOcenuPromena(string jMBG, string sifraPredmeta, string ocena);
    }
}
