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

namespace NNProjekat.Controllers
{
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
                Console.WriteLine("SORT COLUMN: "+sortColumn);
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
            model.Predmet = _predmetData.Vrati(id);
            return View(model);
        }

        [Route("/Student/VratiStudentePoPredmetu/{id}")]
        [HttpPost]
        public IActionResult VratiStudentePoPredmetu(string id)
        {
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
            var model = _studentData.UcitajSvePoPredmetu(id);
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
            model.AktivnostiStudenta = _aktivnostiData.UcitajSvePoStudentuIPredmetu(JMBG, sifraPredmeta);
            return new ViewAsPdf("PrikazStudentaPDF", model) { PageOrientation = Orientation.Landscape, CustomSwitches = "--viewport-size 1000x1000" };
        }
        [Route("/Student/ZakljuciOcenu/{JMBG}/{SifraPredmeta}")]
        public IActionResult ZakljuciOcenu(string JMBG, string sifraPredmeta)
        {
            _slusanjaData.ZakljuciOcenu(JMBG, sifraPredmeta);
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