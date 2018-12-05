using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NNProjekat.Models;
using NNProjekat.Services;
using NNProjekat.ViewModels;

namespace NNProjekat.Controllers
{
    [Authorize]
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

        [Route("/Aktivnost/Dodaj/{sifraPredmeta}/{sifraTipaAktivnosti}/{JMBG}/{JMBGS}")]
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
            }
            else
            {
                model.Predmeti = VratiPredmeteZaCB(JMBGS);
            }
            if (sifraPredmeta != "undefined")
            {
                model.TipoviAktivnosti = _predmetData.Vrati(sifraPredmeta).TipoviAktivnosti;
            }
            else
            {
                model.TipoviAktivnosti = _tipAktivnostiData.UcitajSve();
            }
            if (sifraPredmeta == null || sifraPredmeta == "null")
            {
                model.Studenti = _studentData.UcitajSve();
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
            aktivnost.StudentJMBG = model.StudentJMBG;
            aktivnost.NastavnikJMBG = model.NastavnikJMBG;
            aktivnost.SifraTipaAktivnosti = model.SifraTipaAktivnosti;
            aktivnost.SifraPredmeta = model.SifraPredmeta;
            aktivnost.BrojPoena = model.BrojPoena;
            
            aktivnost.Datum = model.Datum;
            aktivnost.TipAktivnosti = _tipAktivnostiData.VratiTip(model.SifraPredmeta, model.SifraTipaAktivnosti);
            List<Aktivnost> aktivnosti = _aktivnostData.Ucitaj(model.StudentJMBG, model.SifraPredmeta, model.SifraTipaAktivnosti).ToList();
            foreach (Aktivnost aktivnost1 in aktivnosti) {
                aktivnost1.Validna = false;
                _aktivnostData.Izbrisi(aktivnost1);
            }
            if (aktivnost.TipAktivnosti.Obavezna == true)
            {
                Console.WriteLine("obavezna je true");
                if (model.BrojPoena >= aktivnost.TipAktivnosti.MaxBrojPoena * 0.5)
                {
                    Console.WriteLine("br poena true");
                    aktivnost.Status = true;
                }
                else
                {
                    Console.WriteLine("br poena false");
                    aktivnost.Status = false;
                }
            }
            else {
                aktivnost.Status = true;
            }
            aktivnost.Validna = true;
            _aktivnostData.Dodaj(aktivnost);
            _slusanjaData.IzracunajOcenu(model.StudentJMBG, model.SifraPredmeta, _aktivnostData.UcitajSvePoStudentuIPredmetu(model.StudentJMBG, model.SifraPredmeta));
            return RedirectToAction("SviPredmeti", "Predmet");
        }

        [Route("/Aktivnost/AktivnostiPoStudentu/{sifraPredmeta}/{JMBG}")]
        public IActionResult AktivnostiPoStudentu(string sifraPredmeta, string JMBG)
        {
            AktivnostiPoStudentu model = new AktivnostiPoStudentu();
            model.JMBG = JMBG;
            model.SifraPredmeta = sifraPredmeta;
            return View(model);
        }

        [Route("/Aktivnost/VratiAktivnosti/{sifraPredmeta}/{JMBGS}")]
        [HttpPost]
        public IActionResult VratiAktivnosti(string sifraPredmeta, string JMBGS)
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
            List<Aktivnost> aktivnosti = model.ToList();
            
            for (int i = 0; i < aktivnosti.Count; i++) {
                Aktivnost aktivnost = aktivnosti.ElementAt(i);
                Slusa slusa = _slusanjaData.Vrati(aktivnost.StudentJMBG, aktivnost.SifraPredmeta);

                if (slusa.ZakljucenaOcena != null)
                {
                    aktivnosti.Remove(aktivnost);
                }
            }
            model = aktivnosti;
            return View(model);
        }

        [Route("/Aktivnost/SveAktivnostiPost/")]
        [HttpPost]
        public IActionResult SveAktivnostiPost()
        {

            var model = _aktivnostData.UcitajSve().Where(a => a.Validna == true);
            Console.WriteLine("ovo pravi problem");

            foreach (Aktivnost aktivnost in model) {
              
                    Slusa slusa = _slusanjaData.Vrati(aktivnost.StudentJMBG, aktivnost.SifraPredmeta);
                    if (slusa.ZakljucenaOcena != null)
                    {
                        model = model.Where(m => m.StudentJMBG != aktivnost.StudentJMBG || m.NastavnikJMBG != aktivnost.NastavnikJMBG ||
                        m.Datum != aktivnost.Datum ||
                        m.SifraPredmeta != aktivnost.SifraPredmeta ||
                        m.SifraTipaAktivnosti != aktivnost.SifraTipaAktivnosti);
                    }
               
               
            }

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

            /*   if (!string.IsNullOrEmpty(searchValue))
               {
                   Console.WriteLine("pretragaaa "+searchValue);
                   model = model.Where(m => m.Student.BrojIndeksa.StartsWith(searchValue, true, null));
               }*/
            // recordsTotal = model.Count();
            //  var data = model.Skip(skip).Take(pageSize).ToList();
            var data = model.ToList();
            return Json(new { data = data });
        }

        [Route("/Aktivnost/Izmeni/{StudentJMBG}/{SifraPredmeta}/{SifraTipaAktivnosti}/{Datum}/{NastavnikJMBG}")]
        public IActionResult Izmeni()
        {

            var model = _aktivnostData.UcitajSve();
            return View(model);
        }

        public List<Predmet> VratiPredmeteZaCB(string id)
        {
            IEnumerable<Slusa> slusanja = new List<Slusa>();
            slusanja = _slusanjaData.UcitajSve(id);
            List<Predmet> predmeti = new List<Predmet>();
            foreach (Slusa slusa in slusanja)
            {
                predmeti.Add(slusa.Predmet);
            }
            return predmeti;
        }
        public List<Student> VratiStudenteZaCB(string id)
        {
            IEnumerable<Slusa> slusanja = new List<Slusa>();
            slusanja = _studentData.UcitajSvePoPredmetu(id).Where(s => s.ZakljucenaOcena == null);
            List<Student> studenti = new List<Student>();
            foreach (Slusa slusa in slusanja)
            {
                studenti.Add(slusa.Student);
            }
            return studenti;
        }


        [Route("/Aktivnost/Izmena/{JMBG}/{JMBGS}/{sifraPredmeta}/{sifraTipaAktivnosti}/{datum}/{brojPoena}")]
        [HttpGet]
        public IActionResult Izmena(string JMBG, string JMBGS, string sifraPredmeta, string sifraTipaAktivnosti, string datum, string brojPoena)
        {
            AktivnostIzmeni model = new AktivnostIzmeni();
            model.SifraTipaAktivnosti = sifraTipaAktivnosti;
            model.SifraPredmeta = sifraPredmeta;
            model.StudentJMBG = JMBGS;
            model.NastavnikJMBG = JMBG;
            model.IzabraniNastavnik = _nastavnikData.Vrati(JMBG);
            model.IzabraniStudent = _studentData.VratiPoJMBG(JMBGS);
            model.TipAktivnosti = _tipAktivnostiData.VratiTip(sifraPredmeta, sifraTipaAktivnosti);
            model.Datum = DateTime.Parse(datum);
            model.BrojPoena = Int32.Parse(brojPoena);
            return View("Izmeni", model);
        }

        [Route("/Aktivnost/IzmeniPost")]
        [HttpPost]
        public IActionResult IzmeniPost(AktivnostIzmeni model)
        {
            Aktivnost aktivnost = new Aktivnost();
            aktivnost.StudentJMBG = model.StudentJMBG;
            aktivnost.NastavnikJMBG = model.NastavnikJMBG;
            aktivnost.SifraTipaAktivnosti = model.SifraTipaAktivnosti;
            aktivnost.SifraPredmeta = model.SifraPredmeta;
            aktivnost.BrojPoena = model.BrojPoena;
            aktivnost.Datum = model.Datum;
            aktivnost.TipAktivnosti = _tipAktivnostiData.VratiTip(model.SifraPredmeta, model.SifraTipaAktivnosti);
            if (aktivnost.TipAktivnosti.Obavezna == true)
            {
                if (model.BrojPoena >= aktivnost.TipAktivnosti.MaxBrojPoena * 0.5)
                    aktivnost.Status = true;
                else
                    aktivnost.Status = false;
            }
            else
            {
                aktivnost.Status = true;
            }
            aktivnost.Validna = true;
            _aktivnostData.Izmeni(aktivnost);
            _slusanjaData.IzracunajOcenu(model.StudentJMBG, model.SifraPredmeta, _aktivnostData.UcitajSvePoStudentuIPredmetu(model.StudentJMBG, model.SifraPredmeta));
            return RedirectToAction("SveAktivnosti");
        }

        [Route("/Aktivnost/Izbrisi/{JMBG}/{JMBGS}/{sifraPredmeta}/{sifraTipaAktivnosti}/{datum}/{brojPoena}")]
        [HttpGet]
        public IActionResult Izbrisi(string JMBG, string JMBGS, string sifraPredmeta, string sifraTipaAktivnosti, string datum, string brojPoena)
        {
            AktivnostIzmeni model = new AktivnostIzmeni();
            model.SifraTipaAktivnosti = sifraTipaAktivnosti;
            model.SifraPredmeta = sifraPredmeta;
            model.StudentJMBG = JMBGS;
            model.NastavnikJMBG = JMBG;
            model.IzabraniNastavnik = _nastavnikData.Vrati(JMBG);
            model.IzabraniStudent = _studentData.VratiPoJMBG(JMBGS);
            model.TipAktivnosti = _tipAktivnostiData.VratiTip(sifraPredmeta, sifraTipaAktivnosti);
            model.Datum = DateTime.Parse(datum);
            model.BrojPoena = Int32.Parse(brojPoena);
            return View("Izbrisi", model);
        }

        [Route("/Aktivnost/IzbrisiPost")]
        [HttpPost]
        public IActionResult IzbrisiPost(AktivnostIzmeni model)
        {
            Aktivnost aktivnost = new Aktivnost();
            aktivnost.StudentJMBG = model.StudentJMBG;
            aktivnost.NastavnikJMBG = model.NastavnikJMBG;
            aktivnost.SifraTipaAktivnosti = model.SifraTipaAktivnosti;
            aktivnost.SifraPredmeta = model.SifraPredmeta;
            aktivnost.BrojPoena = model.BrojPoena;
            aktivnost.Datum = model.Datum;
            aktivnost.TipAktivnosti = _tipAktivnostiData.VratiTip(model.SifraPredmeta, model.SifraTipaAktivnosti);
            if (aktivnost.TipAktivnosti.Obavezna == true)
            {
                if (model.BrojPoena >= aktivnost.TipAktivnosti.MaxBrojPoena * 0.5)
                    aktivnost.Status = true;
                else
                    aktivnost.Status = false;
            }
            else
            {
                aktivnost.Status = true;
            }
            aktivnost.Validna = false;
            Console.WriteLine("briseeee! "+aktivnost.SifraPredmeta);
            _aktivnostData.Izbrisi(aktivnost);
            _slusanjaData.IzracunajOcenu(model.StudentJMBG, model.SifraPredmeta, _aktivnostData.UcitajSvePoStudentuIPredmetu(model.StudentJMBG, model.SifraPredmeta));
            return RedirectToAction("SveAktivnosti");
        }
        [Route("/Aktivnost/Prikazi/{JMBGS}/{sifraPredmeta}/{sifraTipaAktivnosti}/{datum}")]
        [HttpGet]
        public IActionResult Prikazi(string JMBGS, string sifraPredmeta, string sifraTipaAktivnosti, string datum)
        {
            DateTime datum1 = DateTime.Parse(datum);
            var model = _aktivnostData.Vrati(JMBGS, sifraPredmeta, sifraTipaAktivnosti, datum1);
            return View("Prikazi", model);
        }
    }
}