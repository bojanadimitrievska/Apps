using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EShop.Domain.Domain_models
{
    public class Ticket : BaseEntity
    {
        [Required]
        [Display(Name ="Movie name")]
        public string MovieName { get; set; }
        [Display(Name = "Movie billboard")]
        public string MovieBillboard { get; set; }
        [Display(Name = "Movie genre")]
        public string MovieGenre { get; set; }
        [Display(Name = "Movie price")]
        public float TicketPrice { get; set; }
        public int Rating { get; set; }
        public ICollection<TicketsInShoppingCart> TicketsInShoppingCart { get; set; }
    }
}
