using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NNProjekat.Models;
using NNProjekat.Services;

namespace NNProjekat.Controllers
{
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



        [HttpPost]
        public IActionResult VratiTipoveAktivnostiZaCB(string id)
        {
            IEnumerable<TipAktivnosti> tipoviAktivnosti = new List<TipAktivnosti>();
            tipoviAktivnosti = _predmetData.Vrati(id).TipoviAktivnosti;
            SelectList aktivnostiSel = new SelectList(tipoviAktivnosti, "SifraTipaAktivnosti", "Naziv", 0);
            return Json(aktivnostiSel);
        }
        [HttpPost]
        public IActionResult VratiStudenteZaCb(string id) { 
            IEnumerable<Slusa> slusanja = new List<Slusa>();
            slusanja = _slusanjaData.UcitajSveStudente(id);
            List<Student> studenti = new List<Student>();
            foreach (Slusa slusa in slusanja) {
                studenti.Add(slusa.Student);
            }

            SelectList studentiSel = new SelectList(studenti, "JMBG", "BrojIndeksa", 0);
            return Json(studentiSel);
        }
    }
}
