using GroupSpace23.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GroupSpace23.Controllers
{
    public class Abonnementen : Controller
    {
        public IActionResult Index()
        {
            // Voeg hier logica toe om abonnementen op te halen uit een database of ergens anders
            // Voorbeeld:
            var abonnementen = new List<AbonnementModel>
            {
                new AbonnementModel { AbonnementId = 1, Abonnement= "Standaard" },
                new AbonnementModel { AbonnementId = 2, Abonnement= "Premium" },
                // Voeg andere abonnementen toe zoals nodig
            };

            return View(abonnementen);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(AbonnementModel model)
        {
            if (ModelState.IsValid)
            {
                if (IsValidAbonnement(model.Abonnement))
                {
                    // Voer de logica uit om het abonnement op te slaan of andere acties uit te voeren
                    // ...

                    // Toon een succesbericht
                    TempData["SuccessMessage"] = "Abonnement succesvol toegevoegd!";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Toon een aangepaste foutmelding als het abonnement ongeldig is
                    ModelState.AddModelError("Abonnement", "Het geselecteerde abonnement is momenteel niet beschikbaar. Probeer het later opnieuw.");
                }
            }

            // Als de modelstate niet geldig is, toon het formulier opnieuw met fouten
            return View(model);
        }

        private bool IsValidAbonnement(string abonnement)
        {
            // Plaats hier je logica om te controleren of het abonnement geldig is
            // Je zou bijvoorbeeld een lijst van geldige abonnementen hebben en controleren of de geselecteerde erbij zit.
            // Hier is een eenvoudig voorbeeld:
            var geldigeAbonnementen = new List<string> { "Standaard", "Premium" };
            return geldigeAbonnementen.Contains(abonnement);
        }
    }
}
