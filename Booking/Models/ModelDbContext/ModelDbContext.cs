using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Booking
{
    public partial class ModelDbContext : DbContext
    {
        public ModelDbContext()
            : base("name=ModelDbContext")
        {
        }

        public virtual DbSet<Camera> Camera { get; set; }
        public virtual DbSet<Preferiti> Preferiti { get; set; }
        public virtual DbSet<Prenotazione> Prenotazione { get; set; }
        public virtual DbSet<Recensione> Recensione { get; set; }
        public virtual DbSet<Servizi> Servizi { get; set; }
        public virtual DbSet<Struttura> Struttura { get; set; }
        public virtual DbSet<Utente> Utente { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Camera>()
                .Property(e => e.Prezzo)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Camera>()
                .HasMany(e => e.Prenotazione)
                .WithRequired(e => e.Camera)
                .HasForeignKey(e => e.IdCameraFk)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Camera>()
                .HasMany(e => e.Servizi)
                .WithMany(e => e.Camera)
                .Map(m => m.ToTable("Camere_Servizi").MapLeftKey("IdCameraFk").MapRightKey("IdServizioFk"));

            modelBuilder.Entity<Prenotazione>()
                .Property(e => e.Totale)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Struttura>()
                .HasMany(e => e.Camera)
                .WithRequired(e => e.Struttura)
                .HasForeignKey(e => e.IdStrutturaFk)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Utente>()
                .HasMany(e => e.Prenotazione)
                .WithRequired(e => e.Utente)
                .HasForeignKey(e => e.IdUtenteFk)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Utente>()
                .HasMany(e => e.Recensione)
                .WithRequired(e => e.Utente)
                .HasForeignKey(e => e.IdUtenteFk)
                .WillCascadeOnDelete(false);
        }
    }
}
