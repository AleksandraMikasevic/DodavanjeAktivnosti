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
    public class PolagaoController : Controller
    {
        private IPolaganjaData _polagaoData;
        private IAktivnostData _aktivnostData;
        private IStudentData _studentData;
        private IPredmetData _predmetData;
        private INastavnikData _nastavnikData;

        public PolagaoController(IPolaganjaData polagaoData, IStudentData studentData, IAktivnostData aktivnostData, INastavnikData nastavnikData, IPredmetData predmetData)
        {
            _polagaoData = polagaoData;
            _aktivnostData = aktivnostData;
            _studentData = studentData;
            _predmetData = predmetData;
            _nastavnikData = nastavnikData;
        }

        [Route("/Polagao/Dodaj/{sifraPredmeta}/{sifraAktivnosti}/{JMBG}/{BrojIndeksa}")]
        [HttpGet]
        public IActionResult Dodaj(string sifraPredmeta, string sifraAktivnosti, string JMBG, string BrojIndeksa)
        {
            PolagaoDodaj model = new PolagaoDodaj();
            model.SifraAktivnosti = sifraAktivnosti;
            model.SifraPredmeta = sifraPredmeta;
            model.JMBG = JMBG;
            model.BrojIndeksa = BrojIndeksa;
            model.Nastavnici = _nastavnikData.UcitajSve();
            model.Predmeti = _predmetData.UcitajSve();
            Console.WriteLine(sifraPredmeta+"SIFRA PReDMetAAAAAAAAAAAAAAA-------------");
            if (sifraPredmeta != "undefined")
            {
                model.Aktivnosti = _predmetData.Vrati(sifraPredmeta).Aktivnosti;
            }
            else
            {
                model.Aktivnosti = _aktivnostData.UcitajSve();
            }
            Console.WriteLine(model.Nastavnici.ToList().Count+"===============DUZINA LISTE");
            model.Studenti = _studentData.UcitajSve();
            return View(model);
        }

        [Route("/Polagao/DodajPost")]
        [HttpPost]
        public IActionResult DodajPost(PolagaoDodaj model)
        {
            Polagao polagao = new Polagao();
            Console.WriteLine(model.BrojPoena+"BROJ POENAAAAAAAAAAAA");
            Console.WriteLine(model.JMBG+"Jmbggggggggggggggggggggggggggggggggg");
            polagao.BrojIndeksa = model.BrojIndeksa;
            polagao.JMBG = model.JMBG;
            polagao.SifraAktivnosti = model.SifraAktivnosti;
            polagao.SifraPredmeta = model.SifraPredmeta;
            polagao.BrojPoena = model.BrojPoena;
            Console.WriteLine(model.Datum+" -**********************************-MODEL __________________ DATUM");
            polagao.Datum = model.Datum;
            polagao.Status = true;
            _polagaoData.Dodaj(polagao);
            return RedirectToAction("SviPredmeti", "Predmet");
        }

        [Route("/Polagao/PolaganjaPoStudentu/{sifraPredmeta}/{brojIndeksa}")]
        public IActionResult PolaganjaPoStudentu(string sifraPredmeta, string brojIndeksa)
        {
            PolagaoPolaganjaPoStudentu model = new PolagaoPolaganjaPoStudentu();
            model.BrojIndeksa = brojIndeksa;
            model.SifraPredmeta = sifraPredmeta;
            return View(model);
        }

        [Route("/Polagao/VratiPolaganja/{sifraPredmeta}/{brojIndeksa}")]
        [HttpPost]
        public IActionResult VratiPolaganja(string sifraPredmeta, string brojIndeksa)
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

            var model = _polagaoData.Vrati(sifraPredmeta, brojIndeksa);
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                var sortProperty = typeof(Polagao).GetProperty(sortColumn);
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
                model = model.Where(m => m.BrojIndeksa.StartsWith(
                    searchValue, true, null));
            }
            recordsTotal = model.Count();
            var data = model.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }


    }
}