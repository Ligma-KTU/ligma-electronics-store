using System.ComponentModel.DataAnnotations;

namespace ligma_electronics_store.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "The field Name should only include letters and number.")]
        [Required]
        public string Name { get; set; }

        [Url(ErrorMessage = "Invalid URL format.")]
        [Required]
        public string ImageUrl { get; set; }
    }
}
