namespace Booking
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Recensione")]
    public partial class Recensione
    {
        [Key]
        public int IdRecensione { get; set; }

        public int Punteggio { get; set; }

        [Required]
        public string Commento { get; set; }

        public int IdUtenteFk { get; set; }

        public virtual Utente Utente { get; set; }
    }
}
