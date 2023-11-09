namespace Booking
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Struttura")]
    public partial class Struttura
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Struttura()
        {
            Camera = new HashSet<Camera>();
        }

        [Key]
        public int IdStruttura { get; set; }

        [Required]
        [StringLength(30)]
        public string Nome { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string Descrizione { get; set; }

        [Required]
        [StringLength(30)]
        public string Indirizzo { get; set; }

        [Required]
        [StringLength(20)]
        public string Citta { get; set; }

        [StringLength(30)]
        public string Immagine { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Camera> Camera { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recensione> Recensioni { get; set; }
    }
}
