using GroupSpace23.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Permissions;

namespace GroupSpace23.Models
{
    public class AbonnementModel
    {
        [Display(Name = "AbonnementId")]
        public int AbonnementId { get; set; }

        [Display(Name = "Abonnement")]
        public string Abonnement { get; set; }

        [Display(Name = "Prijs")]
        public decimal Prijs { get; set; }

        [Display(Name = "Details")]
        public string Details { get; set; }
    }
}
