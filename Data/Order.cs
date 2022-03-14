using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPShopBag.Data
{
    public class Order
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public DateTime OrderedOn { get; set; }
    }
}
