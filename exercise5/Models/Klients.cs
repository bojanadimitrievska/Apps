using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Laboratoriska5NEw.Models
{
    public class Klients
    {
        public int ID { set; get; }
        [Required]
        public string Ime { set; get; }
        [Required]
        public string MestoZiveenje { set; get; }
    }
}