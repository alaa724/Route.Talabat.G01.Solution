using System.ComponentModel.DataAnnotations;

namespace AdminDashboard.Models
{
    public class ProductViewModel
    {

		public int id { get; set; }

        [Required(ErrorMessage = "Name Is Requried!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description Is Requried!")]
        public string Description { get; set; }

        public string PictureUrl { get; set; }

        [Required(ErrorMessage = "Price Is Requried!")]
        [Range(1, 10000)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "BrandId Is Requried!")]
        public int BrandId { get; set; }
        public string Brand { get; set; }

    }
}
