namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Camera",
                c => new
                    {
                        IdCamera = c.Int(nullable: false, identity: true),
                        Tipologia = c.String(nullable: false, maxLength: 20),
                        Prezzo = c.Decimal(nullable: false, storeType: "money"),
                        IdStrutturaFk = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdCamera)
                .ForeignKey("dbo.Struttura", t => t.IdStrutturaFk)
                .Index(t => t.IdStrutturaFk);
            
            CreateTable(
                "dbo.Prenotazione",
                c => new
                    {
                        IdPrenotazione = c.Int(nullable: false, identity: true),
                        DataPrenotazione = c.DateTime(nullable: false, storeType: "date"),
                        DataInizio = c.DateTime(nullable: false, storeType: "date"),
                        DataFine = c.DateTime(nullable: false, storeType: "date"),
                        IdUtenteFk = c.Int(nullable: false),
                        IdCameraFk = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdPrenotazione)
                .ForeignKey("dbo.Utente", t => t.IdUtenteFk)
                .ForeignKey("dbo.Camera", t => t.IdCameraFk)
                .Index(t => t.IdUtenteFk)
                .Index(t => t.IdCameraFk);
            
            CreateTable(
                "dbo.Utente",
                c => new
                    {
                        IdUtente = c.Int(nullable: false, identity: true),
                        Cognome = c.String(nullable: false, maxLength: 20),
                        Nome = c.String(nullable: false, maxLength: 20),
                        Email = c.String(nullable: false, maxLength: 30),
                        Username = c.String(nullable: false, maxLength: 20),
                        Password = c.String(nullable: false, maxLength: 20),
                        Ruolo = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.IdUtente);
            
            CreateTable(
                "dbo.Recensione",
                c => new
                    {
                        IdRecensione = c.Int(nullable: false, identity: true),
                        Punteggio = c.Int(nullable: false),
                        Commento = c.String(nullable: false),
                        IdUtenteFk = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdRecensione)
                .ForeignKey("dbo.Utente", t => t.IdUtenteFk)
                .Index(t => t.IdUtenteFk);
            
            CreateTable(
                "dbo.Servizi",
                c => new
                    {
                        IdServizio = c.Int(nullable: false, identity: true),
                        TipoServizio = c.String(nullable: false, maxLength: 30),
                    })
                .PrimaryKey(t => t.IdServizio);
            
            CreateTable(
                "dbo.Struttura",
                c => new
                    {
                        IdStruttura = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 30),
                        Descrizione = c.String(maxLength: 30),
                        Indirizzo = c.String(nullable: false, maxLength: 30),
                        Citta = c.String(nullable: false, maxLength: 20),
                        Immagine = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.IdStruttura);
            
            CreateTable(
                "dbo.Camere_Servizi",
                c => new
                    {
                        IdCameraFk = c.Int(nullable: false),
                        IdServizioFk = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.IdCameraFk, t.IdServizioFk })
                .ForeignKey("dbo.Camera", t => t.IdCameraFk, cascadeDelete: true)
                .ForeignKey("dbo.Servizi", t => t.IdServizioFk, cascadeDelete: true)
                .Index(t => t.IdCameraFk)
                .Index(t => t.IdServizioFk);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Camera", "IdStrutturaFk", "dbo.Struttura");
            DropForeignKey("dbo.Camere_Servizi", "IdServizioFk", "dbo.Servizi");
            DropForeignKey("dbo.Camere_Servizi", "IdCameraFk", "dbo.Camera");
            DropForeignKey("dbo.Prenotazione", "IdCameraFk", "dbo.Camera");
            DropForeignKey("dbo.Recensione", "IdUtenteFk", "dbo.Utente");
            DropForeignKey("dbo.Prenotazione", "IdUtenteFk", "dbo.Utente");
            DropIndex("dbo.Camere_Servizi", new[] { "IdServizioFk" });
            DropIndex("dbo.Camere_Servizi", new[] { "IdCameraFk" });
            DropIndex("dbo.Recensione", new[] { "IdUtenteFk" });
            DropIndex("dbo.Prenotazione", new[] { "IdCameraFk" });
            DropIndex("dbo.Prenotazione", new[] { "IdUtenteFk" });
            DropIndex("dbo.Camera", new[] { "IdStrutturaFk" });
            DropTable("dbo.Camere_Servizi");
            DropTable("dbo.Struttura");
            DropTable("dbo.Servizi");
            DropTable("dbo.Recensione");
            DropTable("dbo.Utente");
            DropTable("dbo.Prenotazione");
            DropTable("dbo.Camera");
        }
    }
}
