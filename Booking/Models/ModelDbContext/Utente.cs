namespace Booking
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Text;

    [Table("Utente")]
    public partial class Utente
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Utente()
        {
            Preferiti = new HashSet<Preferiti>();
            Prenotazione = new HashSet<Prenotazione>();
            Recensione = new HashSet<Recensione>();
        }

        [Key]
        public int IdUtente { get; set; }

        [Required]
        [StringLength(20)]
        public string Cognome { get; set; }

        [Required]
        [StringLength(20)]
        public string Nome { get; set; }

        [Required]
        [StringLength(30)]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        public string Username { get; set; }

        [Required]
        [StringLength(64)]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(172)]
        public string Salt { get; set; }

        [Required]
        [StringLength(10)]
        public string Ruolo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Preferiti> Preferiti { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Prenotazione> Prenotazione { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Recensione> Recensione { get; set; }

        public void SetPassword(string password)
        {
            // Generate a new salt
            var saltBytes = new byte[128]; // 128 bytes = 1024 bits
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }

            Salt = Convert.ToBase64String(saltBytes);

            // Combine the salt and password
            var saltedPassword = Salt + password;

            // Hash the salted password
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));

                // Convert byte array to a string
                PasswordHash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public bool CheckPassword(string password)
        {
            // Combine the salt and password
            var saltedPassword = Salt + password;

            // Hash the salted password
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(saltedPassword));
                var passwordHash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

                return PasswordHash == passwordHash;
            }
        }
    }
}
