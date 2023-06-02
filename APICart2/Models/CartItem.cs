using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace APICart2.Models
{
    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ItemId { get; set; }
        public int Quantity { get; set; }


        public int ProductId { get; set; }
        public int CartId { get; set; }
        public virtual Product Product { get; set; }
        //? commented just to break the cycle of the json size
        //public virtual Cart Cart { get; set; }
    }
}
