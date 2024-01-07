using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BisleriumCafe.Data.Model
{
    public class AddIn
    {
        public Guid addInId { get; set; } = Guid.NewGuid();
        public string addInName { get; set; }
        public string addInDescription { get; set; }
        public decimal addInPrice { get; set; }
    }
}
