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
                        Immagine = c.String(maxLength: 30),
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
                        Totale = c.Decimal(nullable: false, storeType: "money"),
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
                        PasswordHash = c.String(nullable: false, maxLength: 64),
                        Salt = c.String(nullable: false, maxLength: 172),
                        Ruolo = c.String(nullable: false, maxLength: 10),
                    })
                .PrimaryKey(t => t.IdUtente);
            
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
            
            CreateTable(
                "dbo.Struttura",
                c => new
                    {
                        IdStruttura = c.Int(nullable: false, identity: true),
                        Nome = c.String(nullable: false, maxLength: 30),
                        Descrizione = c.String(),
                        Indirizzo = c.String(nullable: false, maxLength: 30),
                        Citta = c.String(nullable: false, maxLength: 20),
                        Immagine = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.IdStruttura);
            
            CreateTable(
                "dbo.Recensione",
                c => new
                    {
                        IdRecensione = c.Int(nullable: false, identity: true),
                        Punteggio = c.Int(nullable: false),
                        Titolo = c.String(nullable: false, maxLength: 30),
                        Commento = c.String(nullable: false),
                        IdUtenteFk = c.Int(nullable: false),
                        IdStruttura = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.IdRecensione)
                .ForeignKey("dbo.Struttura", t => t.IdStruttura, cascadeDelete: true)
                .ForeignKey("dbo.Utente", t => t.IdUtenteFk)
                .Index(t => t.IdUtenteFk)
                .Index(t => t.IdStruttura);
            
            CreateTable(
                "dbo.Servizi",
                c => new
                    {
                        IdServizio = c.Int(nullable: false, identity: true),
                        TipoServizio = c.String(nullable: false, maxLength: 30),
                        Icona = c.String(maxLength: 30),
                    })
                .PrimaryKey(t => t.IdServizio);
            
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
            DropForeignKey("dbo.Camere_Servizi", "IdServizioFk", "dbo.Servizi");
            DropForeignKey("dbo.Camere_Servizi", "IdCameraFk", "dbo.Camera");
            DropForeignKey("dbo.Prenotazione", "IdCameraFk", "dbo.Camera");
            DropForeignKey("dbo.Recensione", "IdUtenteFk", "dbo.Utente");
            DropForeignKey("dbo.Prenotazione", "IdUtenteFk", "dbo.Utente");
            DropForeignKey("dbo.Preferiti", "IdUtente", "dbo.Utente");
            DropForeignKey("dbo.Recensione", "IdStruttura", "dbo.Struttura");
            DropForeignKey("dbo.Preferiti", "IdStruttura", "dbo.Struttura");
            DropForeignKey("dbo.Camera", "IdStrutturaFk", "dbo.Struttura");
            DropIndex("dbo.Camere_Servizi", new[] { "IdServizioFk" });
            DropIndex("dbo.Camere_Servizi", new[] { "IdCameraFk" });
            DropIndex("dbo.Recensione", new[] { "IdStruttura" });
            DropIndex("dbo.Recensione", new[] { "IdUtenteFk" });
            DropIndex("dbo.Preferiti", new[] { "IdStruttura" });
            DropIndex("dbo.Preferiti", new[] { "IdUtente" });
            DropIndex("dbo.Prenotazione", new[] { "IdCameraFk" });
            DropIndex("dbo.Prenotazione", new[] { "IdUtenteFk" });
            DropIndex("dbo.Camera", new[] { "IdStrutturaFk" });
            DropTable("dbo.Camere_Servizi");
            DropTable("dbo.Servizi");
            DropTable("dbo.Recensione");
            DropTable("dbo.Struttura");
            DropTable("dbo.Preferiti");
            DropTable("dbo.Utente");
            DropTable("dbo.Prenotazione");
            DropTable("dbo.Camera");
        }
    }
}
