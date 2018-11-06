using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.ViewModels
{
    public class Ocena
    {
        public int DataOcena { get; set; }
        public int BrojStudenata { get; set; }
        public Ocena(int ocena, int brojStudenata)
        {
            this.DataOcena = ocena;
            this.BrojStudenata = brojStudenata;
        }
    }
}
