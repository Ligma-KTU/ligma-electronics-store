using System.ComponentModel.DataAnnotations;

namespace ligma_electronics_store.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ShoppingCartId { get; set; }

        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "The field Name should only include letters and number.")]
        [Required]
        public string Name { get; set; }

        [DataType(DataType.Currency)]
        [Required]
        public decimal Price { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public int CategoryId { get; set; }
    }
}