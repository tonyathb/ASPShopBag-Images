using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace ASPShopBag.Models
{
    public class OrdersVM
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

       // [Required(ErrorMessage = "This field is required")]
        public List<SelectListItem> Products { get; set; }

        public string UserId { get; set; }

       // [Required(ErrorMessage = "This field is required")]
       // public List<SelectListItem> Users { get; set; }
       
        [Required(ErrorMessage = "This field is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата на закупуване: ")]
        public DateTime OrderedOn { get; set; }
    }
}
