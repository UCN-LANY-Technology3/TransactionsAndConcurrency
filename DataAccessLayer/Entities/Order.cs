using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public required string CustomerName { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public decimal Discount { get; set; }
        public required string Status { get; set; } 
        public IList<Orderline> Orderlines { get; set; } = [];
    }
}
