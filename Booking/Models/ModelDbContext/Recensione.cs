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
        [StringLength(30)]
        public string Titolo { get; set; }

        [Required]
        public string Commento { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Data { get; set; }

        public int IdUtenteFk { get; set; }

        public int IdStruttura { get; set; }

        public virtual Struttura Struttura { get; set; }

        public virtual Utente Utente { get; set; }
    }
}
