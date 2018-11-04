using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NNProjekat.Models;
using NNProjekat.Services;
using NNProjekat.ViewModels;

namespace NNProjekat.Controllers
{
    public class AktivnostController : Controller
    {
        private IAktivnostiData _aktivnostData;
        private ITipAktivnostiData _tipAktivnostiData;
        private ISlusanjaData _slusanjaData;
        private IStudentData _studentData;
        private IPredmetData _predmetData;
        private INastavnikData _nastavnikData;

        public AktivnostController(ISlusanjaData slusanjaData, IAktivnostiData aktivnostData, IStudentData studentData, ITipAktivnostiData tipAktivnostiData, INastavnikData nastavnikData, IPredmetData predmetData)
        {
            _studentData = studentData;
            _predmetData = predmetData;
            _nastavnikData = nastavnikData;
            _aktivnostData = aktivnostData;
            _tipAktivnostiData = tipAktivnostiData;
            _slusanjaData = slusanjaData;
        }

        [Route("/Aktivnost/Dodaj/{sifraPredmeta}/{sifraAktivnosti}/{JMBG}/{JMBGS}")]
        [HttpGet]
        public IActionResult Dodaj(string sifraPredmeta, string sifraTipaAktivnosti, string JMBG, string JMBGS)
        {
            AktivnostDodaj model = new AktivnostDodaj();
            model.SifraTipaAktivnosti = sifraTipaAktivnosti;
            model.SifraPredmeta = sifraPredmeta;
            model.StudentJMBG = JMBGS;
            model.NastavnikJMBG = JMBG;
            model.Nastavnici = _nastavnikData.UcitajSve();
            if (JMBGS == null || JMBGS == "null")
            {
                model.Predmeti = _predmetData.UcitajSve();

                Console.WriteLine("JMBG JE "+JMBGS);
            }
            else
            {
                Console.WriteLine(" ELSE JMBGS JE " + JMBGS);

                model.Predmeti = VratiPredmeteZaCB(JMBGS);
            }
            Console.WriteLine(model.Predmeti.ToList().Count + " SIFRA PReDMetAAAAAAAAAAAAAAA-------------");
            if (sifraPredmeta != "undefined")
            {
                model.TipoviAktivnosti = _predmetData.Vrati(sifraPredmeta).TipoviAktivnosti;
            }
            else
            {
                model.TipoviAktivnosti = _tipAktivnostiData.UcitajSve();
            }
            Console.WriteLine(model.Nastavnici.ToList().Count + "===============DUZINA LISTE");
            if (sifraPredmeta == null || sifraPredmeta == "null")
            {
                model.Studenti = _studentData.UcitajSve();

                Console.WriteLine("SIFRA PREDMETA JE NULL");
            }
            else
            {
                model.Studenti = VratiStudenteZaCB(sifraPredmeta);

            }
            return View(model);
        }

        [Route("/Aktivnost/DodajPost")]
        [HttpPost]
        public IActionResult DodajPost(AktivnostDodaj model)
        {
            Aktivnost aktivnost = new Aktivnost();
            Console.WriteLine(model.BrojPoena + "BROJ POENAAAAAAAAAAAA");
            Console.WriteLine(model.NastavnikJMBG + "Jmbggggggggggggggggggggggggggggggggg");
            aktivnost.StudentJMBG = model.StudentJMBG;
            aktivnost.NastavnikJMBG = model.NastavnikJMBG;
            Console.WriteLine("Sifra tipa aktivnosti ================ " + model.SifraTipaAktivnosti);
            aktivnost.SifraTipaAktivnosti = model.SifraTipaAktivnosti;
            aktivnost.SifraPredmeta = model.SifraPredmeta;
            aktivnost.BrojPoena = model.BrojPoena;
            Console.WriteLine(model.Datum + " -**********************************-MODEL __________________ DATUM");
            aktivnost.Datum = model.Datum;
            aktivnost.Status = true;
            _aktivnostData.Dodaj(aktivnost);
            Console.WriteLine("SIFRA PREDMETAAAA: " + model.SifraPredmeta);
            _slusanjaData.IzracunajOcenu(model.StudentJMBG, model.SifraPredmeta, _aktivnostData.UcitajSvePoStudentuIPredmetu(model.StudentJMBG, model.SifraPredmeta));
            return RedirectToAction("SviPredmeti", "Predmet");
        }

        [Route("/Aktivnost/AktivnostiPoStudentu/{sifraPredmeta}/{JMBG}")]
        public IActionResult AktivnostiPoStudentu(string sifraPredmeta, string JMBG)
        {
            AktivnostiPoStudentu model = new AktivnostiPoStudentu();
            model.JMBG = JMBG;
            model.SifraPredmeta = sifraPredmeta;
            Console.WriteLine("Aktivnosti po studentu get");
            Console.WriteLine(JMBG + "- JMBG");
            Console.WriteLine(sifraPredmeta + "- sifra predmeta");
            return View(model);
        }

        [Route("/Aktivnost/VratiAktivnosti/{sifraPredmeta}/{JMBGS}")]
        [HttpPost]
        public IActionResult VratiAktivnosti(string sifraPredmeta, string JMBGS)
        {
            Console.WriteLine("VRATI POLAGANJAAAAAAAAAAAA------------------------------------------1");
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            Console.WriteLine("VRATI POLAGANJAAAAAAAAAAAA------------------------------------------2");

            var model = _aktivnostData.Vrati(sifraPredmeta, JMBGS);
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                var sortProperty = typeof(Aktivnost).GetProperty(sortColumn);
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
                Console.WriteLine("SEARCH VALUEEE--------------------------------------" + searchValue);
                model = model.Where(m => m.Student.JMBG.StartsWith(
                    searchValue, true, null));
            }
            recordsTotal = model.Count();
            var data = model.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }
        [Route("/Aktivnost/SveAktivnosti/")]
        public IActionResult SveAktivnosti()
        {
            var model = _aktivnostData.UcitajSve();
            return View(model);
        }

        [Route("/Aktivnost/SveAktivnostiPost/")]
        [HttpPost]
        public IActionResult SveAktivnostiPost()
        {
            Console.WriteLine("VRATI POLAGANJAAAAAAAAAAAA------------------------------------------1");
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            Console.WriteLine("VRATI POLAGANJAAAAAAAAAAAA------------------------------------------2");

            var model = _aktivnostData.UcitajSve();
            /* if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
             {
                 var sortProperty = typeof(Aktivnost).GetProperty(sortColumn);
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
                Console.WriteLine("SEARCH VALUEEE--------------------------------------" + searchValue);
                model = model.Where(m => m.Student.JMBG.StartsWith(
                    searchValue, true, null));
            }
            recordsTotal = model.Count();
            var data = model.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        [Route("/Aktivnost/Izmeni/{StudentJMBG}/{SifraPredmeta}/{SifraTipaAktivnosti}/{Datum}/{NastavnikJMBG}")]
        public IActionResult Izmeni()
        {
            var model = _aktivnostData.UcitajSve();
            return View(model);
        }

        public List<Predmet> VratiPredmeteZaCB(string id)
        {
            Console.WriteLine("JMBG JE "+id);
            IEnumerable<Slusa> slusanja = new List<Slusa>();
            slusanja = _slusanjaData.UcitajSve(id);
            List<Predmet> predmeti = new List<Predmet>();
            foreach (Slusa slusa in slusanja)
            {
                predmeti.Add(slusa.Predmet);
            }
            Console.WriteLine("DUZINA PREDMETA JE :"+predmeti.Count);
            return predmeti;
        }
        public List<Student> VratiStudenteZaCB(string id)
        {
            IEnumerable<Slusa> slusanja = new List<Slusa>();
            slusanja = _studentData.UcitajSvePoPredmetu(id);
            List<Student> studenti = new List<Student>();
            foreach (Slusa slusa in slusanja)
            {
                studenti.Add(slusa.Student);
            }
            return studenti;
        }

    }


}