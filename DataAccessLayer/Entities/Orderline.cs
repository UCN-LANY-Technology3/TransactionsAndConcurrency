using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Orderline
    {
        public required Product Item { get; set; }
        public int Quantity { get; set; }
    }
}
