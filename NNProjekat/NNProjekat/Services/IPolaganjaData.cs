using NNProjekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.Services
{
    public interface IPolaganjaData
    {
        IEnumerable<Polagao> UcitajSvePoStudentuIPredmetu(string brojIndeksa, string sifraPredmeta);
        Polagao Dodaj(Polagao polagao);
        Polagao Izmeni(Polagao polagao);
    }
}
