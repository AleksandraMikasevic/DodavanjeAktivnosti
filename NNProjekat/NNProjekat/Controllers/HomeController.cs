﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NNProjekat.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() {
            return View();
        }

        public IActionResult Login()
        {
            return View("Main");
        }
    }
}