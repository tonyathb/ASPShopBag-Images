using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static ASPShopBag.Data.EnumType;

namespace ASPShopBag.Data
{
    public class Product
    {        
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }

        public string Description { get; set; }

        public TypeFood Type { get; set; }

       // public string ImageId { get; set; }
        public ICollection<ProductImages> ProductImages { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
