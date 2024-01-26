using System.ComponentModel.DataAnnotations;

namespace GroupSpace23.Models
{
    public class Leverancier
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "Naam")]
        
        [Required(ErrorMessage = "Deze veld is verplicht")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Deze veld is verplicht")]
        [Display(Name = "Omschrijving")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Deze veld is verplicht")]
        [EmailAddress]
        public string email { get; set; }
        
    }


}