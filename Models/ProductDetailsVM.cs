using ASPShopBag.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASPShopBag.Models
{
    public class ProductDetailsVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public decimal Price { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public TypeFood Type { get; set; }

        public List<string> ImagesPaths { get; set; }
    }
}
