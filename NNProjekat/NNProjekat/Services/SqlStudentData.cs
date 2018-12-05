using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
            _context.Studenti.Add(student);
            _context.SaveChanges();
            return student;
        }

        public Student Dodaj(Student student, List<Predmet> predmeti)
        {
            _context.Studenti.Add(student);
            foreach (Predmet predmet in predmeti)
            {
                Slusa slusa = new Slusa();
                slusa.JMBG = student.JMBG;
                slusa.SifraPredmeta = predmet.SifraPredmeta;
                slusa.ZakljucenaOcena = null;
                slusa.DatumZakljucivanja = null;
                slusa.DatumPrvogUpisa = DateTime.Now;
                slusa.PredlozenaOcena = 0;
                _context.Add(slusa);
            }
            _context.SaveChanges();
            return student;
        }

        public void Izbrisi(string JMBG)
        {
            Student student = _context.Studenti.Where(s => s.JMBG == JMBG).Single();
            var model = _context.Aktivnosti.Where(a => a.StudentJMBG == JMBG);
            foreach (Aktivnost aktivnost in model)
            {
                _context.Remove(aktivnost);
            }
            _context.Studenti.Remove(student);
            _context.SaveChanges();
        }

        public void Izmeni(Student student)
        {
            _context.Studenti.Update(student);
            _context.SaveChanges();
        }

        public Student Izmeni(Student student, List<Predmet> predmeti)
        {
            _context.Studenti.Update(student);
            foreach (Predmet predmet in predmeti)
            {
                bool nadjen = false;
                foreach (Slusa slusa1 in _context.Slusanja) {
                    if (slusa1.JMBG == student.JMBG && slusa1.SifraPredmeta == predmet.SifraPredmeta) {
                        nadjen = true;
                        break;
                    }

                }
                if (nadjen == false)
                {
                    Slusa slusa = new Slusa();
                    slusa.JMBG = student.JMBG;
                    slusa.SifraPredmeta = predmet.SifraPredmeta;
                    slusa.ZakljucenaOcena = null;
                    slusa.DatumZakljucivanja = null;
                    slusa.DatumPrvogUpisa = DateTime.Now;
                    slusa.PredlozenaOcena = 0;
                    _context.Add(slusa);
                }

            }
            _context.SaveChanges();
            return student;
        }

        public IEnumerable<Student> UcitajSve()
        {
            return _context.Studenti.OrderBy(r => r.BrojIndeksa);
        }

        public IEnumerable<Slusa> UcitajSvePoPredmetu(string id)
        {
            return _context.Slusanja.Include(p => p.Student).Include(p => p.Student).Include(p => p.Predmet).Where(p => p.SifraPredmeta == id).OrderBy(p => p.JMBG);
        }

        public Student Vrati(string brojIndeksa)
        {
            return _context.Studenti.FirstOrDefault(r => r.BrojIndeksa == brojIndeksa);
        }

        public Student VratiPoJMBG(string jMBGS)
        {
            return _context.Studenti.FirstOrDefault(r => r.JMBG == jMBGS);
        }
    }
}
