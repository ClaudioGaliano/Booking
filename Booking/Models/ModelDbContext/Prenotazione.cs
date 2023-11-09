namespace Booking
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Prenotazione")]
    public partial class Prenotazione
    {
        [Key]
        public int IdPrenotazione { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataPrenotazione { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataInizio { get; set; }

        [Column(TypeName = "date")]
        public DateTime DataFine { get; set; }

        [Column(TypeName = "money")]
        public decimal Totale { get; set; }

        public int IdUtenteFk { get; set; }

        public int IdCameraFk { get; set; }

        public virtual Camera Camera { get; set; }

        public virtual Utente Utente { get; set; }
    }
}
