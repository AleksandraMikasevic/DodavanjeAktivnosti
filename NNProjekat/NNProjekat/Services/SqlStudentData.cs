using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NNProjekat.Data;
using NNProjekat.Models;

namespace NNProjekat.Services
{
    public class SqlStudentData : IStudentData
    {
        private NNProjekatDbContext _context;

        public SqlStudentData(NNProjekatDbContext context)
        {
            _context = context;
        }

        public Student Dodaj(Student student)
        {
            _context.Add(student);
            _context.SaveChanges();
            return student;
        }

        public IEnumerable<Student> UcitajSve()
        {
            return _context.Studenti.OrderBy(r => r.BrojIndeksa);
        }

        public Student Vrati(string brojIndeksa)
        {
            return _context.Studenti.FirstOrDefault(r => r.BrojIndeksa == brojIndeksa);
        }
    }
}
