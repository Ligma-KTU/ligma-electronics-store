using System.ComponentModel.DataAnnotations;

namespace ligma_electronics_store.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Please enter your address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please enter your zip code")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Please enter your country")]
        public string Country { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Please enter your phone number")]
        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }
    }
}
