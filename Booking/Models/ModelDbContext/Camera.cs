namespace Booking
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Camera")]
    public partial class Camera
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Camera()
        {
            Prenotazione = new HashSet<Prenotazione>();
            Servizi = new HashSet<Servizi>();
        }

        [Key]
        public int IdCamera { get; set; }

        [Required]
        [StringLength(20)]
        public string Tipologia { get; set; }

        [Column(TypeName = "money")]
        public decimal Prezzo { get; set; }

        public int IdStrutturaFk { get; set; }

        public virtual Struttura Struttura { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prenotazione> Prenotazione { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Servizi> Servizi { get; set; }
    }
}
