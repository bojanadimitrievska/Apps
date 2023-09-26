using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace lab4.Models
{
    public class FriendModel
    {
        public int Id { get; set; }
        [Display (Name="Name and surname")]
        [Required]
        public string Ime{ get; set; }
        [Display(Name = "City")]
        [Required]
        public string MestoZiveenje { get; set; }
    }
}