using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Booking
{
    public class Registrazione
    {
        [Required]
        [StringLength(20)]
        public string Cognome { get; set; }

        [Required]
        [StringLength(20)]
        public string Nome { get; set; }

        [Required]
        [StringLength(30)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        //[Required]
        //[StringLength(10)]
        //public string Ruolo { get; set; }
    }
}