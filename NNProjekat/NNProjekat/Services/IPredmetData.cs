using NNProjekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.Services
{
    public interface IPredmetData
    {
        IQueryable<Predmet> UcitajSve();
        Predmet Vrati(string sifraPredmeta);
        Predmet Dodaj(Predmet predmet);
        IEnumerable<TipAktivnosti> VratiIzmeni(string sifraPredmeta);
        void Izmeni(Predmet predmet);
        void Izbrisi(string id);
    }
}
