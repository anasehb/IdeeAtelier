using GroupSpace23.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            // Logica voor het maken van een nieuw abonnement
            return View();
        }
    }
}
