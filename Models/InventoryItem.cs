using GroupSpace23.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;

namespace GroupSpace23.Models
{
    public class InventoryItem
    {
        public int Id { get; set; }

    
        [Required(ErrorMessage = "Deze veld is verplicht")]
        [Display(Name = "Naam")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Deze veld is verplicht")]
        [Display(Name = "Omschrijving")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Deze veld is verplicht")]
        [Display(Name = "Hoeveelheid")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Deze veld is verplicht")]
        [DataType(DataType.Currency)]
        [Display(Name = "Prijs")]
        public decimal Price { get; set; }
        
        [Display(Name = "Geckocht")]
        public bool IsChecked { get; set; }

        public string OwnerId { get; set; }

        //Navigatie-eigenschap naar de eigenaar van het inventarisitem
        public GroupSpace23User Owner { get; set; }


        public int ProjectId { get; set; }
        public Project Project { get; set; }


    }
}
