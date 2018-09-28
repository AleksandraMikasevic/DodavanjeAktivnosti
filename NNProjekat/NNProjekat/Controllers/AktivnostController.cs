using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NNProjekat.Controllers
{
    public class AktivnostController : Controller
    {
        [HttpGet]
        [Route("Aktivnost/Dodaj/{sifraPredmeta}/{sifraAktivnosti}")]
        public IActionResult Dodaj()
        {

            return View();
        }
    }
}