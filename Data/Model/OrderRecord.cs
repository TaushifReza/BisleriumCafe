using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace BisleriumCafe.Data.Model
{
    public class OrderRecord
    {
        public Guid OrderRecordId { get; set; } = Guid.NewGuid();

        //[Required(ErrorMessage = "Please provide Coffee Name!")]
        public List<Coffee> CoffeeList { get; set; }

        //[Required(ErrorMessage = "Please provide AddIn name!")]
        public List<AddIn> AddInList { get; set; }

        public User CustomerIdUser { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
        public decimal NetAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
