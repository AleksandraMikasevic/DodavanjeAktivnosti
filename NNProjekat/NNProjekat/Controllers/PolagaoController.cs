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
        private INastavnikData _nastavnikData;

        public PolagaoController(IPolaganjaData polagaoData, IStudentData studentData, IAktivnostData aktivnostData, INastavnikData nastavnikData)
        {
            _polagaoData = polagaoData;
            _aktivnostData = aktivnostData;
            _studentData = studentData;
            _nastavnikData = nastavnikData;
        }

        [Route("/Polagao/Dodaj/{sifraPredmeta}/{sifraAktivnosti}")]
        [HttpGet]
        public IActionResult Dodaj(string sifraPredmeta, string sifraAktivnosti)
        {
            PolagaoDodaj model = new PolagaoDodaj();
            model.SifraAktivnosti = sifraAktivnosti;
            model.SifraPredmeta = sifraPredmeta;
            model.Nastavnici = _nastavnikData.UcitajSve();
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

        [Route("/Polagao/VratiPolaganja/{sifraPredmeta}/{brojIndeksa}")]
        [HttpPost]
        public IActionResult VratiPolaganja(string sifraPredmeta, string brojIndeksa)
        {
            Console.WriteLine("VRATI AKTIVNOSTI");
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;
            var model = _polagaoData.Vrati(sifraPredmeta, brojIndeksa).Aktivnosti;
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
                model = model.Where(m => m.Naziv.StartsWith(
                    searchValue, true, null));
            }
            recordsTotal = model.Count();
            var data = model.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }


    }
}