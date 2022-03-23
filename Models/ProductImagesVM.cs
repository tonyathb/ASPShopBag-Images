using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ASPShopBag.Models
{
    public class ProductImagesVM
    {
        public ProductImagesVM()
        {
            this.Id = Guid.NewGuid().ToString();
        }
        
        [Key]
        public string Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        public List<SelectListItem> Products { get; set; }

        [Required]
        public IFormFile ImagePath { get; set; }


    }
}
