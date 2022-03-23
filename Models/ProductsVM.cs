using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ASPShopBag.Data.EnumType;

namespace ASPShopBag.Models
{
    public class ProductsVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public decimal Price { get; set; }

        public string Description { get; set; }

        [Required(ErrorMessage = "This field is required")]
        public TypeFood Type { get; set; }

        //[Required]
        //public string ImageId { get; set; }

        [Required(ErrorMessage ="Избери снимка от компютъра си...")]
        public List<IFormFile> ImagePath { get; set; }
    }
}
