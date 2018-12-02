using NNProjekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.Services
{
    public interface INastavnikData
    {
        IEnumerable<Nastavnik> UcitajSve();
        Nastavnik Vrati(string jMBG);
        bool ProveriNastavnika(string username, string password);
        Nastavnik VratiPoUsername(string username);
    }
}
