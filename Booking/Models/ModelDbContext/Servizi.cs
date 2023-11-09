namespace Booking
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Servizi")]
    public partial class Servizi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Servizi()
        {
            Camera = new HashSet<Camera>();
        }

        [Key]
        public int IdServizio { get; set; }

        [Required]
        [StringLength(30)]
        public string TipoServizio { get; set; }

        [StringLength(30)]
        public string Icona { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Camera> Camera { get; set; }
    }
}
