using NNProjekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.Services
{
    public interface IAktivnostData
    {
        IEnumerable<Aktivnost> UcitajSvePoPredmetu(string sifraPredmeta);
        IEnumerable<Aktivnost> UcitajSve();
    }
}
