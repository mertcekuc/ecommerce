using EcommerceApp.Models;
using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Models.ViewModels
{
    public class EditAddressViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Street")]
        public string Street { get; set; }

        [Required]
        [Display(Name = "City")]
        public int CityId { get; set; }

        [Required]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Required]
        [Display(Name = "Postal Code")]
        public int PostalCode { get; set; }

        public List<Country> AvailableCountries { get; set; }
        public List<City> AvailableCities { get; set; }
    }
}