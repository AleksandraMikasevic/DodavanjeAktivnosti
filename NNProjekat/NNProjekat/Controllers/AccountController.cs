using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NNProjekat.Models;
using NNProjekat.Services;
using NNProjekat.ViewModels;

namespace NNProjekat.Controllers
{
    public class AccountController : Controller
    {
        private INastavnikData _nastavnikData;

        public AccountController(INastavnikData nastavnikData)
        {
            _nastavnikData = nastavnikData;
        }
        public IActionResult Index()
        {
            return View("login");
        }

        public async Task<IActionResult> OnPostAsync(AccountLogin model)
        {
            Console.WriteLine("krenuo login");
            if (ModelState.IsValid)
            {
                var isValid = _nastavnikData.ProveriNastavnika(model.Username, model.Password); //proveris iz baze
                Console.WriteLine("isValid:       "+isValid);
                if (!isValid)
                {
                    Console.WriteLine("uslo u greskuuu ");
                    ModelState.AddModelError("error", "Ne postoji nastavnik sa unetim korisničkim imenom i/ili lozinkom.");
                    return View("login");
                }
                Nastavnik nastavnik = _nastavnikData.VratiPoUsername(model.Username);
                // Create the identity from the user info
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme, ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, model.Username));
                identity.AddClaim(new Claim(ClaimTypes.Name, model.Username));
                HttpContext.Session.SetString("nastavnik", nastavnik.Ime + " " + nastavnik.Prezime);
                Console.WriteLine(HttpContext.Session.GetString("nastavnik"));
                // Authenticate using the identity
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties { IsPersistent = model.RememberMe });

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

    }

}