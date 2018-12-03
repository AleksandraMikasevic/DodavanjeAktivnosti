using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NNProjekat.Models;
using NNProjekat.Services;
using NNProjekat.ViewModels;

namespace NNProjekat.Controllers
{
    [Authorize]
    public class PredmetController : Controller
    {
        private IPredmetData _predmetData;
        private ISlusanjaData _slusanjaData;

        public PredmetController(IPredmetData predmetData, ISlusanjaData slusanjaData)
        {
            _predmetData = predmetData;
            _slusanjaData = slusanjaData;
        }
        public IActionResult SviPredmeti()
        {
            Console.WriteLine("Aleksandra");
            var model = _predmetData.UcitajSve();
            return View(model);
        }
        [HttpPost]
        public IActionResult VratiPredmete()
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
            var model = _predmetData.UcitajSve();

            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                Console.WriteLine("prvoooooooooooooooooooooooooooooooo");
                Console.WriteLine("sort column: " + sortColumn);
                var sortProperty = typeof(Predmet).GetProperty(sortColumn);
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
                model = model.Where(m => m.Naziv.StartsWith(searchValue, true, null));
            }
            recordsTotal = model.Count();
            var data = model.Skip(skip).Take(pageSize).ToList();
            return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = data });
        }

        public IActionResult PrikazPredmeta(string id)
        {

            Predmet predmet = _predmetData.Vrati(id);
            return View(predmet);
        }

        [HttpPost]
        public IActionResult VratiTipoveAktivnosti(string id)
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
            var model = _predmetData.Vrati(id).TipoviAktivnosti;
            if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDirection)))
            {
                var sortProperty = typeof(TipAktivnosti).GetProperty(sortColumn);
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
        public IActionResult Dodaj()
        {
            PredmetDodaj predmet = new PredmetDodaj();
            return View(predmet);
        }
        [HttpGet]
        public IActionResult VratiTipoveAktivnostiDodaj()
        {
            var model = new List<TipAktivnostiDodaj>();
            return Json(new { data = model });
        }
        [HttpGet]
        public IActionResult VratiTipoveAktivnostiIzmeni(string id)
        {
            var model = _predmetData.Vrati(id).TipoviAktivnosti;
            return Json(new { data = model });
        }
        [HttpGet]
        [Route("/Predmet/DodajTip/{Naziv}/{MinBrojPoena}/{MaxBrojPoena}/{TezKoef}/{Obavezna}")]
        public IActionResult DodajAktivnost(string Naziv, string MinBrojPoena, string MaxBrojPoena, string TezKoef, string Obavezna)
        {
            Console.WriteLine("Naziv: "+Naziv+", min: "+MinBrojPoena+" maks: "+MaxBrojPoena+" obavezna: "+Obavezna);
            TipAktivnosti tipAktivnosti = new TipAktivnosti();
            tipAktivnosti.SifraTipaAktivnosti = Guid.NewGuid().ToString();
            tipAktivnosti.Naziv = Naziv;
            tipAktivnosti.MinBrojPoena = Double.Parse(MinBrojPoena);
            tipAktivnosti.MaxBrojPoena = Double.Parse(MaxBrojPoena);
            tipAktivnosti.TezinskiKoeficijent = Double.Parse(TezKoef);
            tipAktivnosti.Obavezna = Boolean.Parse(Obavezna);
            //dodas u json
            return Json(new { data = tipAktivnosti });
        }
        [HttpGet]
        [Route("/Predmet/IzmeniTip/{SifraTipa}/{Naziv}/{MinBrojPoena}/{MaxBrojPoena}/{TezKoef}/{Obavezna}")]
        public IActionResult IzmeniAktivnost(string SifraTipa, string Naziv, string MinBrojPoena, string MaxBrojPoena, string TezKoef, string Obavezna)
        {
            Console.WriteLine("Naziv: " + Naziv + ", min: " + MinBrojPoena + " maks: " + MaxBrojPoena + " obavezna: " + Obavezna);
            TipAktivnosti tipAktivnosti = new TipAktivnosti();
            //pronadjes tip aktivnosti iz jsona i promenis onaj sa tom sifrom tipa
            tipAktivnosti.SifraTipaAktivnosti = SifraTipa;
            tipAktivnosti.Naziv = Naziv;
            tipAktivnosti.MinBrojPoena = Double.Parse(MinBrojPoena);
            tipAktivnosti.MaxBrojPoena = Double.Parse(MaxBrojPoena);
            tipAktivnosti.TezinskiKoeficijent = Double.Parse(TezKoef);
            tipAktivnosti.Obavezna = Boolean.Parse(Obavezna);
            return Json(new { data = tipAktivnosti });
        }
        [HttpGet]
        public IActionResult IzbrisiAktivnost(string id)
        {
            TipAktivnosti tipAktivnosti = new TipAktivnosti();
            //pronadjes tip aktivnosti iz jsona i i izbrises onog sa tom sifrom
            return Json(new { data = tipAktivnosti });
        }

        [HttpPost]
        public IActionResult DodajPost(PredmetDodaj predmetDodaj)
        {
            Predmet predmet = new Predmet();
            predmet.Naziv = predmetDodaj.Naziv;
            predmet.SifraPredmeta = predmetDodaj.SifraPredmeta;
            predmet.BrojESPB = predmetDodaj.BrojESPB;
            JArray nizTipova = JArray.Parse(predmetDodaj.JsonString);
            predmet.TipoviAktivnosti = nizTipova.ToObject<List<TipAktivnosti>>();
            _predmetData.Dodaj(predmet);
            return RedirectToAction("SviPredmeti", "Predmet");
        }

        /*
       [HttpPost]
        public IActionResult VratiTipoveAktivnostiDodaj(ISession session)
        {
            var predmetJson = session.GetString("predmet");
            Predmet predmet = JsonConvert.DeserializeObject<Predmet>(predmetJson);
            return Json(new { data = predmet.TipoviAktivnosti.ToList() });
        }
        
        public IActionResult DodajPredmet(ISession session)
        {
            Predmet predmet = new Predmet();
            session.SetString("predmet", JsonConvert.SerializeObject(predmet));
            return View(predmet);
        }

        [HttpPost]
        public IActionResult VratiTipoveAktivnostiIzmeni(String id)
        {
            var model = _predmetData.Vrati(id).TipoviAktivnosti;
            var data = model.ToList();
            return Json(new { data = data });
        }
        */
        /*
        [HttpPost]
        public IActionResult DodajAktivnost(ISession session, TipAktivnosti tipAktivnosti)
        {
            var predmetJson = session.GetString("predmet");
            Predmet predmet = JsonConvert.DeserializeObject<Predmet>(predmetJson);
            predmet.TipoviAktivnosti.ToList().Add(tipAktivnosti);
        }
        [HttpPost]
        public IActionResult IzmeniAktivnost(ISession session, TipAktivnosti tipAktivnosti)
        {
            HttpContext.Session.
            var predmetJson = session.GetString("predmet");
            Predmet predmet = JsonConvert.DeserializeObject<Predmet>(predmetJson);
            TipAktivnosti ta = predmet.TipoviAktivnosti.ToList().Where(t => t.SifraTipaAktivnosti == tipAktivnosti.SifraTipaAktivnosti).Single();
            ta = tipAktivnosti;
        }
        [HttpPost]
        public IActionResult IzbrisiAktivnost(ISession session, String id)
        {
            var predmetJson = session.GetString("predmet");
            Predmet predmet = JsonConvert.DeserializeObject<Predmet>(predmetJson);
            TipAktivnosti ta = predmet.TipoviAktivnosti.ToList().Where(t => t.SifraTipaAktivnosti == id).Single();
            predmet.TipoviAktivnosti.ToList().Remove(ta);
        }
        */
        [HttpPost]
        public IActionResult VratiTipoveAktivnostiZaCB(string id)
        {
            IEnumerable<TipAktivnosti> tipoviAktivnosti = new List<TipAktivnosti>();
            if (!id.Equals("0"))
            {
                tipoviAktivnosti = _predmetData.Vrati(id).TipoviAktivnosti;
            }
            SelectList aktivnostiSel = new SelectList(tipoviAktivnosti, "SifraTipaAktivnosti", "Naziv", 0);
            return Json(aktivnostiSel);
        }
        [HttpPost]
        public IActionResult VratiStudenteZaCb(string id)
        {
            IEnumerable<Slusa> slusanja = new List<Slusa>();
            slusanja = _slusanjaData.UcitajSveStudente(id).Where(s => s.ZakljucenaOcena == null);
            List<Student> studenti = new List<Student>();
            foreach (Slusa slusa in slusanja)
            {
                studenti.Add(slusa.Student);
            }
            SelectList studentiSel = new SelectList(studenti.Select(s => new { Text = s.Ime + " " + s.Prezime + "-" + s.BrojIndeksa, JMBG = s.JMBG }), "JMBG", "Text", 0);
            return Json(studentiSel);
        }

        public IActionResult VizuelniPrikaz(string id)
        {
            IEnumerable<Slusa> slusanja = _slusanjaData.UcitajSve();
            slusanja = slusanja.Where(s => s.ZakljucenaOcena != null || s.PredlozenaOcena == 5).Where(s => s.SifraPredmeta == id);
            List<Ocena> ocene = new List<Ocena>();
            ocene = ocene.ToList();
            ocene.Add(new Ocena(5, slusanja.Count(s => s.PredlozenaOcena == 5)));
            ocene.Add(new Ocena(6, slusanja.Count(s => s.ZakljucenaOcena == 6)));
            ocene.Add(new Ocena(7, slusanja.Count(s => s.ZakljucenaOcena == 7)));
            ocene.Add(new Ocena(8, slusanja.Count(s => s.ZakljucenaOcena == 8)));
            ocene.Add(new Ocena(9, slusanja.Count(s => s.ZakljucenaOcena == 9)));
            ocene.Add(new Ocena(10, slusanja.Count(s => s.ZakljucenaOcena == 10)));
            PredmetGrafickiPrikaz predmetGrafickiPrikaz = new PredmetGrafickiPrikaz();
            predmetGrafickiPrikaz.Predmet = _predmetData.Vrati(id);
            predmetGrafickiPrikaz.Ocene = ocene;
            var model = predmetGrafickiPrikaz;
            return View(model);
        }

        public ActionResult DisplaySearchResults(string searchText)
        {
            return PartialView("_DodajTip");
        }
    }
}
