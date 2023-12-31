namespace Booking
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Preferiti")]
    public partial class Preferiti
    {
        [Key]
        public int IdPreferito { get; set; }

        public int IdUtente { get; set; }

        public int IdStruttura { get; set; }

        public virtual Struttura Struttura { get; set; }

        public virtual Utente Utente { get; set; }
    }
}
