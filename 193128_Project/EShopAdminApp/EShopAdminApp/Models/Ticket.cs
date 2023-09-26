using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EShopAdminApp.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public string MovieName { get; set; }
        public string MovieBillboard { get; set; }
        public string MovieGenre { get; set; }
        public float TicketPrice { get; set; }
        public int Rating { get; set; }
    }
}
