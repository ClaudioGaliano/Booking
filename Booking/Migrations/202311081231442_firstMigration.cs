namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class firstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Preferiti",
                c => new
                    {
                        IdPreferito = c.Int(nullable: false, identity: true),
                        IdUtente = c.Int(nullable: false),
                        IdStruttura = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPreferito)
                .ForeignKey("dbo.Struttura", t => t.IdStruttura, cascadeDelete: true)
                .ForeignKey("dbo.Utente", t => t.IdUtente, cascadeDelete: true)
                .Index(t => t.IdUtente)
                .Index(t => t.IdStruttura);
            
            AddColumn("dbo.Camera", "Immagine", c => c.String(maxLength: 30));
            AddColumn("dbo.Prenotazione", "Totale", c => c.Decimal(nullable: false, storeType: "money"));
            AddColumn("dbo.Recensione", "IdStruttura", c => c.Int(nullable: false));
            AddColumn("dbo.Servizi", "Icona", c => c.String(maxLength: 30));
            AlterColumn("dbo.Struttura", "Descrizione", c => c.String());
            CreateIndex("dbo.Recensione", "IdStruttura");
            AddForeignKey("dbo.Recensione", "IdStruttura", "dbo.Struttura", "IdStruttura", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Preferiti", "IdUtente", "dbo.Utente");
            DropForeignKey("dbo.Preferiti", "IdStruttura", "dbo.Struttura");
            DropForeignKey("dbo.Recensione", "IdStruttura", "dbo.Struttura");
            DropIndex("dbo.Recensione", new[] { "IdStruttura" });
            DropIndex("dbo.Preferiti", new[] { "IdStruttura" });
            DropIndex("dbo.Preferiti", new[] { "IdUtente" });
            AlterColumn("dbo.Struttura", "Descrizione", c => c.String(maxLength: 30));
            DropColumn("dbo.Servizi", "Icona");
            DropColumn("dbo.Recensione", "IdStruttura");
            DropColumn("dbo.Prenotazione", "Totale");
            DropColumn("dbo.Camera", "Immagine");
            DropTable("dbo.Preferiti");
        }
    }
}
