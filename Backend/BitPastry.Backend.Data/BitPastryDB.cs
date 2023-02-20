using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BitPastry.Backend.Data
{
    public partial class BitPastryDB : DbContext
    {
        public BitPastryDB()
        {
        }

        public BitPastryDB(DbContextOptions<BitPastryDB> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Bod> Bods { get; set; } = null!;
        public virtual DbSet<Bom> Boms { get; set; } = null!;
        public virtual DbSet<Bop> Bops { get; set; } = null!;
        public virtual DbSet<Consuntivilavorazione> Consuntivilavoraziones { get; set; } = null!;
        public virtual DbSet<Datimateriali> Datimaterialis { get; set; } = null!;
        public virtual DbSet<Datiprocesso> Datiprocessos { get; set; } = null!;
        public virtual DbSet<Lottiproduzione> Lottiproduziones { get; set; } = null!;
        public virtual DbSet<Magazzino> Magazzinos { get; set; } = null!;
        public virtual DbSet<Materieprime> Materieprimes { get; set; } = null!;
        public virtual DbSet<Operatori> Operatoris { get; set; } = null!;
        public virtual DbSet<Ordinilavorazione> Ordinilavoraziones { get; set; } = null!;
        public virtual DbSet<Ricette> Ricettes { get; set; } = null!;
        public virtual DbSet<Semilavorati> Semilavoratis { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_unicode_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(e => e.IdAccount)
                    .HasName("PRIMARY");

                entity.ToTable("accounts");

                entity.HasIndex(e => e.OperatoriIdOperatore, "fk_accounts_operatori1_idx");

                entity.Property(e => e.IdAccount)
                    .HasColumnType("int(11)")
                    .HasColumnName("idAccount");

                entity.Property(e => e.OperatoriIdOperatore)
                    .HasColumnType("int(11)")
                    .HasColumnName("operatori_idOperatore");

                entity.Property(e => e.Password).HasColumnType("text");

                entity.Property(e => e.TsCreazione)
                    .HasColumnType("timestamp")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.HasOne(d => d.OperatoriIdOperatoreNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.OperatoriIdOperatore)
                    .HasConstraintName("fk_accounts_operatori1");
            });

            modelBuilder.Entity<Bod>(entity =>
            {
                entity.HasKey(e => e.IdBod)
                    .HasName("PRIMARY");

                entity.ToTable("bods");

                entity.HasIndex(e => e.IdBop, "fk_bods_bops1_idx");

                entity.Property(e => e.IdBod)
                    .HasColumnType("int(11)")
                    .HasColumnName("idBOD");

                entity.Property(e => e.Descrizione).HasMaxLength(100);

                entity.Property(e => e.IdBop)
                    .HasColumnType("int(11)")
                    .HasColumnName("idBOP");

                entity.Property(e => e.Obbligatorio).HasColumnType("tinyint(4)");

                entity.Property(e => e.UnitaMisura).HasMaxLength(10);

                entity.HasOne(d => d.IdBopNavigation)
                    .WithMany(p => p.Bods)
                    .HasForeignKey(d => d.IdBop)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_bods_bops1");
            });

            modelBuilder.Entity<Bom>(entity =>
            {
                entity.HasKey(e => e.IdBom)
                    .HasName("PRIMARY");

                entity.ToTable("boms");

                entity.HasIndex(e => e.IdBop, "idBOP");

                entity.HasIndex(e => e.IdMateria, "idMateria");

                entity.Property(e => e.IdBom)
                    .HasColumnType("int(11)")
                    .HasColumnName("idBOM");

                entity.Property(e => e.IdBop)
                    .HasColumnType("int(11)")
                    .HasColumnName("idBOP");

                entity.Property(e => e.IdMateria)
                    .HasColumnType("int(11)")
                    .HasColumnName("idMateria");

                entity.HasOne(d => d.IdBopNavigation)
                    .WithMany(p => p.Boms)
                    .HasForeignKey(d => d.IdBop)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("boms_ibfk_1");

                entity.HasOne(d => d.IdMateriaNavigation)
                    .WithMany(p => p.Boms)
                    .HasForeignKey(d => d.IdMateria)
                    .HasConstraintName("boms_ibfk_3");
            });

            modelBuilder.Entity<Bop>(entity =>
            {
                entity.HasKey(e => e.IdBop)
                    .HasName("PRIMARY");

                entity.ToTable("bops");

                entity.HasIndex(e => e.IdRicetta, "idRicetta");

                entity.Property(e => e.IdBop)
                    .HasColumnType("int(11)")
                    .HasColumnName("idBOP");

                entity.Property(e => e.Descrizione).HasColumnType("text");

                entity.Property(e => e.IdRicetta)
                    .HasColumnType("int(11)")
                    .HasColumnName("idRicetta");

                entity.Property(e => e.OrderIndex)
                    .HasColumnType("int(11)")
                    .HasColumnName("Order_Index");

                entity.Property(e => e.Titolo).HasMaxLength(200);

                entity.HasOne(d => d.IdRicettaNavigation)
                    .WithMany(p => p.Bops)
                    .HasForeignKey(d => d.IdRicetta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("bops_ibfk_1");
            });

            modelBuilder.Entity<Consuntivilavorazione>(entity =>
            {
                entity.HasKey(e => e.IdLavorazione)
                    .HasName("PRIMARY");

                entity.ToTable("consuntivilavorazione");

                entity.HasIndex(e => e.IdBop, "idBOP");

                entity.HasIndex(e => e.IdOrdine, "idOrdine");

                entity.HasIndex(e => e.IdOperatore, "lavorazioni_ibfk_3");

                entity.Property(e => e.IdLavorazione)
                    .HasColumnType("int(11)")
                    .HasColumnName("idLavorazione");

                entity.Property(e => e.IdBop)
                    .HasColumnType("int(11)")
                    .HasColumnName("idBOP");

                entity.Property(e => e.IdOperatore)
                    .HasColumnType("int(11)")
                    .HasColumnName("idOperatore");

                entity.Property(e => e.IdOrdine)
                    .HasColumnType("int(11)")
                    .HasColumnName("idOrdine");

                entity.Property(e => e.TsFineLavorazione)
                    .HasColumnType("timestamp")
                    .HasColumnName("TS_fine_lavorazione");

                entity.Property(e => e.TsInizioLavorazione)
                    .HasColumnType("timestamp")
                    .HasColumnName("TS_inizio_lavorazione");

                entity.Property(e => e.TsPresaInCarico)
                    .HasColumnType("timestamp")
                    .HasColumnName("TS_presa_in_carico");

                entity.Property(e => e.UnitaMisura).HasMaxLength(10);

                entity.HasOne(d => d.IdBopNavigation)
                    .WithMany(p => p.Consuntivilavoraziones)
                    .HasForeignKey(d => d.IdBop)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lavorazioni_ibfk_2");

                entity.HasOne(d => d.IdOperatoreNavigation)
                    .WithMany(p => p.Consuntivilavoraziones)
                    .HasForeignKey(d => d.IdOperatore)
                    .HasConstraintName("lavorazioni_ibfk_3");

                entity.HasOne(d => d.IdOrdineNavigation)
                    .WithMany(p => p.Consuntivilavoraziones)
                    .HasForeignKey(d => d.IdOrdine)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lavorazioni_ibfk_1");
            });

            modelBuilder.Entity<Datimateriali>(entity =>
            {
                entity.HasKey(e => e.IdRaccoltaMateriali)
                    .HasName("PRIMARY");

                entity.ToTable("datimateriali");

                entity.HasIndex(e => e.IdBom, "idBOM");

                entity.HasIndex(e => e.IdLavorazione, "idLavorazione");

                entity.HasIndex(e => e.IdMagazzino, "idMagazzino");

                entity.HasIndex(e => e.IdSemilavorato, "idSemilavorato");

                entity.Property(e => e.IdRaccoltaMateriali)
                    .HasColumnType("int(11)")
                    .HasColumnName("idRaccoltaMateriali");

                entity.Property(e => e.IdBom)
                    .HasColumnType("int(11)")
                    .HasColumnName("idBOM");

                entity.Property(e => e.IdLavorazione)
                    .HasColumnType("int(11)")
                    .HasColumnName("idLavorazione");

                entity.Property(e => e.IdMagazzino)
                    .HasColumnType("int(11)")
                    .HasColumnName("idMagazzino");

                entity.Property(e => e.IdSemilavorato)
                    .HasColumnType("int(11)")
                    .HasColumnName("idSemilavorato");

                entity.Property(e => e.TsPrelievo)
                    .HasColumnType("timestamp")
                    .HasColumnName("TS_prelievo");

                entity.HasOne(d => d.IdBomNavigation)
                    .WithMany(p => p.Datimaterialis)
                    .HasForeignKey(d => d.IdBom)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("raccoltamateriali_ibfk_3");

                entity.HasOne(d => d.IdLavorazioneNavigation)
                    .WithMany(p => p.Datimaterialis)
                    .HasForeignKey(d => d.IdLavorazione)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("raccoltamateriali_ibfk_1");

                entity.HasOne(d => d.IdMagazzinoNavigation)
                    .WithMany(p => p.Datimaterialis)
                    .HasForeignKey(d => d.IdMagazzino)
                    .HasConstraintName("raccoltamateriali_ibfk_2");

                entity.HasOne(d => d.IdSemilavoratoNavigation)
                    .WithMany(p => p.Datimaterialis)
                    .HasForeignKey(d => d.IdSemilavorato)
                    .HasConstraintName("raccoltamateriali_ibfk_4");
            });

            modelBuilder.Entity<Datiprocesso>(entity =>
            {
                entity.HasKey(e => e.Idraccoltadati)
                    .HasName("PRIMARY");

                entity.ToTable("datiprocesso");

                entity.HasIndex(e => e.IdBod, "fk_raccoltadati_bods1_idx");

                entity.HasIndex(e => e.Idlavorazione, "fk_raccoltadati_lavorazioni1_idx");

                entity.Property(e => e.Idraccoltadati)
                    .HasColumnType("int(11)")
                    .HasColumnName("idraccoltadati");

                entity.Property(e => e.IdBod)
                    .HasColumnType("int(11)")
                    .HasColumnName("idBOD");

                entity.Property(e => e.Idlavorazione)
                    .HasColumnType("int(11)")
                    .HasColumnName("idlavorazione");

                entity.Property(e => e.Tipo)
                    .HasColumnType("int(11)")
                    .HasColumnName("tipo");

                entity.Property(e => e.Ts)
                    .HasColumnType("timestamp")
                    .HasColumnName("TS");

                entity.Property(e => e.Valore)
                    .HasMaxLength(45)
                    .HasColumnName("valore");

                entity.HasOne(d => d.IdBodNavigation)
                    .WithMany(p => p.Datiprocessos)
                    .HasForeignKey(d => d.IdBod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_raccoltadati_bods1");

                entity.HasOne(d => d.IdlavorazioneNavigation)
                    .WithMany(p => p.Datiprocessos)
                    .HasForeignKey(d => d.Idlavorazione)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_raccoltadati_lavorazioni1");
            });

            modelBuilder.Entity<Lottiproduzione>(entity =>
            {
                entity.HasKey(e => e.IdLotto)
                    .HasName("PRIMARY");

                entity.ToTable("lottiproduzione");

                entity.HasIndex(e => e.IdLavorazione, "idLavorazione");

                entity.Property(e => e.IdLotto)
                    .HasColumnType("int(11)")
                    .HasColumnName("idLotto");

                entity.Property(e => e.CodiceInterno).HasMaxLength(20);

                entity.Property(e => e.IdLavorazione)
                    .HasColumnType("int(11)")
                    .HasColumnName("idLavorazione");

                entity.Property(e => e.Quantità).HasColumnType("int(11)");

                entity.Property(e => e.TsConfezionamento)
                    .HasColumnType("timestamp")
                    .HasColumnName("TS_Confezionamento");

                entity.HasOne(d => d.IdLavorazioneNavigation)
                    .WithMany(p => p.Lottiproduziones)
                    .HasForeignKey(d => d.IdLavorazione)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("lottiproduzione_ibfk_1");
            });

            modelBuilder.Entity<Magazzino>(entity =>
            {
                entity.HasKey(e => e.IdMagazzino)
                    .HasName("PRIMARY");

                entity.ToTable("magazzino");

                entity.HasIndex(e => e.IdMateria, "idMateria");

                entity.Property(e => e.IdMagazzino)
                    .HasColumnType("int(11)")
                    .HasColumnName("idMagazzino");

                entity.Property(e => e.DataArrivoInMagazzino).HasColumnName("Data_arrivo_in_magazzino");

                entity.Property(e => e.DataOrdine).HasColumnName("Data_ordine");

                entity.Property(e => e.DataSmaltimento).HasColumnName("Data_smaltimento");

                entity.Property(e => e.IdMateria)
                    .HasColumnType("int(11)")
                    .HasColumnName("idMateria");

                entity.Property(e => e.IdentificativoLottoMateriaPrima)
                    .HasMaxLength(200)
                    .HasColumnName("Identificativo_lotto_materia_prima");

                entity.Property(e => e.PesoAvanzato).HasColumnName("Peso_avanzato");

                entity.Property(e => e.QuantitaColli)
                    .HasColumnType("int(11)")
                    .HasColumnName("Quantita_colli");

                entity.Property(e => e.TsFineUtilizzo)
                    .HasColumnType("timestamp")
                    .HasColumnName("TS_fine_utilizzo");

                entity.Property(e => e.TsInizioUtilizzo)
                    .HasColumnType("timestamp")
                    .HasColumnName("TS_inizio_utilizzo");

                entity.Property(e => e.UnitaMisura).HasMaxLength(10);

                entity.Property(e => e.UnitaPerCollo)
                    .HasColumnType("int(11)")
                    .HasColumnName("Unita_per_collo");

                entity.HasOne(d => d.IdMateriaNavigation)
                    .WithMany(p => p.Magazzinos)
                    .HasForeignKey(d => d.IdMateria)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("magazzino_ibfk_1");
            });

            modelBuilder.Entity<Materieprime>(entity =>
            {
                entity.HasKey(e => e.IdMateria)
                    .HasName("PRIMARY");

                entity.ToTable("materieprime");

                entity.HasIndex(e => e.IdRicetta, "idRicetta");

                entity.Property(e => e.IdMateria)
                    .HasColumnType("int(11)")
                    .HasColumnName("idMateria");

                entity.Property(e => e.IdRicetta)
                    .HasColumnType("int(11)")
                    .HasColumnName("idRicetta");

                entity.Property(e => e.Nome).HasMaxLength(200);

                entity.Property(e => e.UnitaMisura).HasMaxLength(10);

                entity.HasOne(d => d.IdRicettaNavigation)
                    .WithMany(p => p.Materieprimes)
                    .HasForeignKey(d => d.IdRicetta)
                    .HasConstraintName("materieprime_ibfk_1");
            });

            modelBuilder.Entity<Operatori>(entity =>
            {
                entity.HasKey(e => e.IdOperatore)
                    .HasName("PRIMARY");

                entity.ToTable("operatori");

                entity.Property(e => e.IdOperatore)
                    .HasColumnType("int(11)")
                    .HasColumnName("idOperatore");

                entity.Property(e => e.Cognome).HasMaxLength(50);

                entity.Property(e => e.Contatto).HasMaxLength(100);

                entity.Property(e => e.Livello).HasColumnType("int(11)");

                entity.Property(e => e.Matricola).HasColumnType("int(11)");

                entity.Property(e => e.Nome).HasMaxLength(50);

                entity.Property(e => e.TsInserimento)
                    .HasColumnType("timestamp")
                    .HasColumnName("TS_Inserimento")
                    .HasDefaultValueSql("current_timestamp()");
            });

            modelBuilder.Entity<Ordinilavorazione>(entity =>
            {
                entity.HasKey(e => e.IdOrdine)
                    .HasName("PRIMARY");

                entity.ToTable("ordinilavorazione");

                entity.HasIndex(e => e.IdRicetta, "idRicetta");

                entity.HasIndex(e => e.IdOperatore, "ordini_ibfk_3");

                entity.Property(e => e.IdOrdine)
                    .HasColumnType("int(11)")
                    .HasColumnName("idOrdine");

                entity.Property(e => e.ContattoCliente)
                    .HasMaxLength(100)
                    .HasColumnName("Contatto_Cliente");

                entity.Property(e => e.IdDestinatarioCliente)
                    .HasColumnType("int(11)")
                    .HasColumnName("idDestinatarioCliente");

                entity.Property(e => e.IdOperatore)
                    .HasColumnType("int(11)")
                    .HasColumnName("idOperatore");

                entity.Property(e => e.IdRicetta)
                    .HasColumnType("int(11)")
                    .HasColumnName("idRicetta");

                entity.Property(e => e.Quantità).HasColumnType("int(11)");

                entity.Property(e => e.RifCliente)
                    .HasMaxLength(100)
                    .HasColumnName("Rif_Cliente");

                entity.Property(e => e.TsFineOrdine)
                    .HasColumnType("timestamp")
                    .HasColumnName("TS_fine_ordine");

                entity.Property(e => e.TsOrdine)
                    .HasColumnType("timestamp")
                    .HasColumnName("TS_ordine");

                entity.HasOne(d => d.IdOperatoreNavigation)
                    .WithMany(p => p.Ordinilavoraziones)
                    .HasForeignKey(d => d.IdOperatore)
                    .HasConstraintName("ordini_ibfk_3");

                entity.HasOne(d => d.IdRicettaNavigation)
                    .WithMany(p => p.Ordinilavoraziones)
                    .HasForeignKey(d => d.IdRicetta)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("ordini_ibfk_1");
            });

            modelBuilder.Entity<Ricette>(entity =>
            {
                entity.HasKey(e => e.IdRicetta)
                    .HasName("PRIMARY");

                entity.ToTable("ricette");

                entity.Property(e => e.IdRicetta)
                    .HasColumnType("int(11)")
                    .HasColumnName("idRicetta");

                entity.Property(e => e.Autore).HasMaxLength(200);

                entity.Property(e => e.Descrizione).HasColumnType("text");

                entity.Property(e => e.Nome).HasMaxLength(200);

                entity.Property(e => e.TsInserimento)
                    .HasColumnType("timestamp")
                    .HasColumnName("TS_Inserimento")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.UnitaMisura).HasMaxLength(10);
            });

            modelBuilder.Entity<Semilavorati>(entity =>
            {
                entity.HasKey(e => e.IdSemilavorato)
                    .HasName("PRIMARY");

                entity.ToTable("semilavorati");

                entity.HasIndex(e => e.IdLavorazione, "idLavorazione");

                entity.Property(e => e.IdSemilavorato)
                    .HasColumnType("int(11)")
                    .HasColumnName("idSemilavorato");

                entity.Property(e => e.IdLavorazione)
                    .HasColumnType("int(11)")
                    .HasColumnName("idLavorazione");

                entity.Property(e => e.TsStoccaggio)
                    .HasColumnType("timestamp")
                    .HasColumnName("TS_stoccaggio");

                entity.Property(e => e.UnitaMisura).HasMaxLength(10);

                entity.HasOne(d => d.IdLavorazioneNavigation)
                    .WithMany(p => p.Semilavoratis)
                    .HasForeignKey(d => d.IdLavorazione)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("semilavorati_ibfk_1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
