using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data.Model
{
    public class Coffee
    {
        public Guid coffeeId { get; set; } = Guid.NewGuid();
        public string coffeeName { get; set; }
        public string coffeeDescription { get; set;}
        public decimal coffeePrice { get; set; }
    }
}
