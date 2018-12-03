using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.ViewModels
{
    public class StudentSviStudentiMod
    {
        public string Naziv { get; set; }
        public string Vrednost { get; set; }

        public StudentSviStudentiMod(string Naziv, string Vrednost)
        {
            this.Naziv = Naziv;
            this.Vrednost = Vrednost;
        }
        public StudentSviStudentiMod()
        {
                
        }
    }
}
