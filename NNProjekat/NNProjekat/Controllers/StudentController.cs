using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NNProjekat.Models;
using NNProjekat.Services;
using NNProjekat.ViewModels;
using Rotativa.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace NNProjekat.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        private IStudentData _studentData;
        private IPredmetData _predmetData;
        private ISlusanjaData _slusanjaData;
        private IAktivnostiData _aktivnostiData;
        private ITipAktivnostiData _tipAktivnostiData;


        public StudentController(ITipAktivnostiData tipAktivnostiData, IAktivnostiData aktivnostiData, IStudentData studentData, IPredmetData predmetData, ISlusanjaData slusanjaData)
        {
            _studentData = studentData;
            _predmetData = predmetData;
            _slusanjaData = slusanjaData;
            _aktivnostiData = aktivnostiData;
            _tipAktivnostiData = tipAktivnostiData;

        }

        public IActionResult SviStudenti()
        {
            Console.WriteLine("Studenti");
            var model = _studentData.UcitajSve();
            return View(model);
        }

        [HttpPost]
        public IActionResult VratiStudente()
        {
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var model = _studentData.UcitajSve();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                Console.WriteLine("SORT COLUMN: " + sortColumn);
                var sortProperty = typeof(Student).GetProperty(sortColumn);
                if (sortColumnDirection == "asc")
                {
                    model = model.OrderBy(p => sortProperty.GetValue(p, null));
                }
                if (sortColumnDirection == "desc")
                {
                    model = model.OrderByDescending(p => sortProperty.GetValue(p, null));
                }
            }

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(m => m.BrojIndeksa.StartsWith(searchValue, true, null));
            }
            recordsTotal = model.Count();
            var data = model.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        public IActionResult StudentiPoPredmetu(string id)
        {
            Console.WriteLine("Studenti");
            var model = new StudentSviStudenti();
            model.Studenti = _studentData.UcitajSvePoPredmetu(id);
            model.SifraPredmeta = id;
            model.Mod = new List<StudentSviStudentiMod>();
            model.Mod.Add(new StudentSviStudentiMod("Položili", "0"));
            model.Mod.Add(new StudentSviStudentiMod("Svi studenti", "1"));
            model.Mod.Add(new StudentSviStudentiMod("Nisu položili", "2"));

            model.Predmet = _predmetData.Vrati(id);
            return View(model);
        }

        public IActionResult Dodaj()
        {
            var model = new StudentDodaj();
            return View(model);
        }
        [HttpPost]
        public IActionResult DodajPost(StudentDodaj studentDodaj)
        {
            Student student = new Student();
            student.Ime = studentDodaj.Ime;
            student.Prezime = studentDodaj.Prezime;
            student.JMBG = studentDodaj.JMBG;
            student.BrojIndeksa = studentDodaj.BrojIndeksa;
            _studentData.Dodaj(student);
            return RedirectToAction("SviStudenti", "Student");
        }

        [Route("/Student/IzaberiPredmet/{id}")]
        public IActionResult IzaberiPredmet(string id)
        {
            Student student = _studentData.VratiPoJMBG(id);
            IEnumerable<Slusa> slusanja = new List<Slusa>();
            slusanja = _slusanjaData.UcitajSve(id);
            List<Predmet> predmeti = new List<Predmet>();
            foreach (Slusa slusa in slusanja)
            {
                predmeti.Add(slusa.Predmet);
            }
            List<Predmet> sviPredmeti = _predmetData.UcitajSve().ToList();
            List<Predmet> preostaliPredmeti = new List<Predmet>();

            foreach (Predmet predmet in sviPredmeti)
            {
                bool pronadjen = false;
                foreach (Predmet predmet1 in predmeti)
                {
                    if (predmet1.SifraPredmeta == predmet.SifraPredmeta)
                    {
                        pronadjen = true;
                        break;
                    }
                }
                if (pronadjen == false)
                    preostaliPredmeti.Add(predmet);
            }
            var model = new SlusanjeDodaj();
            model.Predmeti = preostaliPredmeti;
            model.Student = student;
            return View(model);
        }
        [Route("/Student/PredmetiStudent/{id}")]
        [HttpPost]
        public IActionResult PredmetiStudent(string id)
        {
            var model = _slusanjaData.UcitajSve(id).Where(s => s.ZakljucenaOcena == null);
            Console.WriteLine("ID: ++++++++++++++++++" + id);
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            Console.WriteLine(model.ToList().Count + "------------------------------DUZINA SLUSANJAAA");
            Console.WriteLine("SOrt colummmmmmmmmmmmn --" + sortColumn);
            /* if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
             {
                 var sortProperty = typeof(Slusa).GetProperty(sortColumn);
                 if (sortColumnDirection == "asc")
                 {
                     model = model.OrderBy(p => sortProperty.GetValue(p, null));
                 }
                 if (sortColumnDirection == "desc")
                 {
                     model = model.OrderByDescending(p => sortProperty.GetValue(p, null));
                 }
             }*/

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(m => m.JMBG.StartsWith(searchValue, true, null));
            }
            recordsTotal = model.Count();
            var data = model.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [Route("/Student/DodajSlusanje/{sifraPredmeta}/{JMBG}")]
        public void DodajSlusanje(string sifraPredmeta, string JMBG)
        {
            Console.WriteLine("Dodaj slusanje");
            Slusa slusa = new Slusa();
            slusa.SifraPredmeta = sifraPredmeta;
            slusa.JMBG = JMBG;
            slusa.PredlozenaOcena = 0;
            slusa.ZakljucenaOcena = null;
            slusa.DatumPrvogUpisa = new DateTime();
            slusa.DatumZakljucivanja = null;
            _slusanjaData.Sacuvaj(slusa);
        }
        [Route("/Student/IzbrisiSlusanje/{sifraPredmeta}/{JMBG}")]
        public void IzbrisiSlusanje(string sifraPredmeta, string JMBG)
        {
            Console.WriteLine("Izbrisi slusanje slusanje");
            Slusa slusa = _slusanjaData.Vrati(JMBG, sifraPredmeta);
            _slusanjaData.Izbrisi(slusa);
        }
        public IActionResult IzmeniStudenta(string id)
        {
            var model = _studentData.VratiPoJMBG(id);
            return View("Izmeni", model);
        }
        public IActionResult IzbrisiStudenta(string id)
        {
            var model = _studentData.VratiPoJMBG(id);
            return View("Izbrisi", model);
        }
        [HttpPost]
        public IActionResult IzmeniPost(string id, Student model)
        {
            Console.WriteLine(id + " id studenta za izmenu");
            model.JMBG = id;
            Console.WriteLine("Menja studenta***************");
            _studentData.Izmeni(model);
            return RedirectToAction("SviStudenti", "Student");
        }
        [HttpPost]
        public IActionResult IzbrisiPost(string id)
        {
            _studentData.Izbrisi(id);
            return RedirectToAction("SviStudenti", "Student");
        }
        public IActionResult VratiPreostalePredmete(string id)
        {
            Console.WriteLine("Vrati preostale predmete");

            IEnumerable<Slusa> slusanja = new List<Slusa>();
            slusanja = _slusanjaData.UcitajSve(id);
            List<Predmet> predmeti = new List<Predmet>();
            foreach (Slusa slusa in slusanja)
            {
                predmeti.Add(slusa.Predmet);
            }
            List<Predmet> sviPredmeti = _predmetData.UcitajSve().ToList();
            List<Predmet> preostaliPredmeti = new List<Predmet>();

            foreach (Predmet predmet in sviPredmeti)
            {
                bool pronadjen = false;
                foreach (Predmet predmet1 in predmeti)
                {
                    if (predmet1.SifraPredmeta == predmet.SifraPredmeta)
                    {
                        pronadjen = true;
                        break;
                    }
                }
                if (pronadjen == false)
                    preostaliPredmeti.Add(predmet);
            }
            SelectList predmetiSel = new SelectList(preostaliPredmeti, "SifraPredmeta", "Naziv", 0);
            return Json(predmetiSel);
        }
        [Route("/Student/VratiStudentePoPredmetu/{id}/{datumOd}/{datumDo}/{mod}")]
        [HttpPost]
        public IActionResult VratiStudentePoPredmetu(string id, string datumOd, string datumDo, string mod)
        {
            Console.WriteLine("VRACAAAAAAAAAAA STUDENTE");
            Console.WriteLine("ID: ++++++++++++++++++" + id);
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var model = _slusanjaData.UcitajSveStudente(id).Where(s => s.ZakljucenaOcena != null);
            if (mod == "1") {
                Console.WriteLine("Svi studenti");
                model = _slusanjaData.UcitajSveStudente(id);
            }
            if (mod == "2") {
                model = _slusanjaData.UcitajSveStudente(id).Where(s => s.ZakljucenaOcena == null);
            }
            if (mod == "0")
            {
                Console.WriteLine(datumOd + " datumOd");
                Console.WriteLine(datumDo + " datumDo");
                if (datumOd != "undefined" && datumDo != "undefined")
                {
                    Console.WriteLine("uslo u undefined");
                    model = _slusanjaData.UcitajSveStudenteIzmedjuDatuma(id, datumOd, datumDo);
                }
            }
            Console.WriteLine(model.ToList().Count + "------------------------------DUZINA SLUSANJAAA");
            Console.WriteLine("SOrt colummmmmmmmmmmmn --" + sortColumn);
            /* if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
             {
                 var sortProperty = typeof(Slusa).GetProperty(sortColumn);
                 if (sortColumnDirection == "asc")
                 {
                     model = model.OrderBy(p => sortProperty.GetValue(p, null));
                 }
                 if (sortColumnDirection == "desc")
                 {
                     model = model.OrderByDescending(p => sortProperty.GetValue(p, null));
                 }
             }*/

            if (!string.IsNullOrEmpty(searchValue))
            {
                model = model.Where(m => m.JMBG.StartsWith(searchValue, true, null));
            }
            recordsTotal = model.Count();
            var data = model.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [Route("/Student/VratiPredmetePoStudentu/{id}")]
        [HttpPost]
        public IActionResult VratiPredmetePoStudentu(string id)
        {
            var model = _slusanjaData.UcitajSve(id);
            Console.WriteLine(model.ToList().Count + "------------------------------DUZINA SLUSANJAAA");
            Console.WriteLine("SLusanja duzinaaaaaaaaaa: " + model.ToList().Count);
            Console.WriteLine(Json(new { recordsFiltered = model.ToList(), recordsTotal = model.ToList(), data = model.ToList() }).Value);
            string o = Newtonsoft.Json.JsonConvert.SerializeObject(Json(new { recordsFiltered = model.ToList(), recordsTotal = model.ToList(), data = model.ToList() }));
            Console.WriteLine("p ----------- " + o);
            return Json(new { recordsFiltered = model.ToList(), recordsTotal = model.ToList(), data = model.ToList() });
        }


        [Route("/Student/StudentPredmetAktivnosti/{JMBG}/{SifraPredmeta}")]
        public IActionResult StudentPredmetAktivnosti(string JMBG, string sifraPredmeta)
        {
            StudentPrikazStudenta model = new StudentPrikazStudenta();
            model.Slusa = _slusanjaData.Vrati(JMBG, sifraPredmeta);
            model.AktivnostiStudenta = _aktivnostiData.UcitajSvePoStudentuIPredmetuPrikaz(JMBG, sifraPredmeta);
            return View("PrikazStudenta", model);
        }
        [Route("/Student/PDF/{JMBG}/{SifraPredmeta}")]
        public IActionResult KreirajPDF(string JMBG, string sifraPredmeta)
        {
            StudentPrikazStudenta model = new StudentPrikazStudenta();
            model.Slusa = _slusanjaData.Vrati(JMBG, sifraPredmeta);
            model.Nastavnik = HttpContext.Session.GetString("nastavnik");
            model.Datum = DateTime.Now;
            Console.WriteLine("U SESSION SE NALAXI: " + model.Nastavnik);
            model.AktivnostiStudenta = _aktivnostiData.UcitajSvePoStudentuIPredmetu(JMBG, sifraPredmeta);
            //return new ViewAsPdf("PrikazStudentaPDF", model) { PageOrientation = Orientation.Landscape, CustomSwitches = "--viewport-size 1000x1000" };
            return new ViewAsPdf("PrikazStudentaPDF", model)
            {
                PageOrientation = Orientation.Landscape,
                CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
            };

        }
        [Route("/Student/ZakljuciOcenu/{JMBG}/{SifraPredmeta}")]
        public IActionResult ZakljuciOcenu(string JMBG, string sifraPredmeta)
        {
            _slusanjaData.ZakljuciOcenu(JMBG, sifraPredmeta);
            return RedirectToAction("StudentPredmetAktivnosti");
        }
        [Route("/Student/IzbrisiZakljucenuOcenu/{JMBG}/{SifraPredmeta}")]
        public IActionResult IzbrisiZakljucenuOcenu(string JMBG, string sifraPredmeta)
        {
            _slusanjaData.ZakljuciOcenuPromena(JMBG, sifraPredmeta,null);
            return RedirectToAction("StudentPredmetAktivnosti");
        }

        [Route("/Student/ZakljuciOcenuPromena/{JMBG}/{SifraPredmeta}")]
        [HttpPost]
        public IActionResult ZakljuciOcenuPromena(string JMBG, string sifraPredmeta, string ocena)
        {
            _slusanjaData.ZakljuciOcenuPromena(JMBG, sifraPredmeta, ocena);
            return RedirectToAction("StudentPredmetAktivnosti");
        }

        [HttpPost]
        public IActionResult VratiPredmeteZaCB(string id)
        {
            IEnumerable<Slusa> slusanja = new List<Slusa>();
            slusanja = _slusanjaData.UcitajSve(id);
            List<Predmet> predmeti = new List<Predmet>();
            foreach (Slusa slusa in slusanja)
            {
                predmeti.Add(slusa.Predmet);
            }

            SelectList predmetiSel = new SelectList(predmeti, "SifraPredmeta", "Naziv", 0);
            return Json(predmetiSel);
        }
    }
}