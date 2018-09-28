using NNProjekat.Models;
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
    }
}
