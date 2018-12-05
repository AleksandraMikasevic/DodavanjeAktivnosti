using NNProjekat.Models;
using NNProjekat.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.Services
{
    public interface IStudentData
    {
        IEnumerable<Student> UcitajSve();
        Student Vrati(string brojIndeksa);
        Student Dodaj(Student student);
        IEnumerable<Slusa> UcitajSvePoPredmetu(string id);
        Student VratiPoJMBG(string jMBGS);
        void Izbrisi(string JMBG);
        void Izmeni(Student student);
        Student Dodaj(Student student, List<Predmet> predmeti);
        Student Izmeni(Student student, List<Predmet> predmeti);
    }
}
