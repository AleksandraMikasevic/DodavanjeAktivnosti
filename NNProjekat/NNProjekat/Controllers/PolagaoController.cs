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
            polagao.Datum = new DateTime();
            polagao.Status = true;
            _polagaoData.Dodaj(polagao);
            return RedirectToAction("SviPredmeti", "Predmet");
        }

    }
}