using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace BackPfe.Models
{
    public partial class BasePfeContext : DbContext
    {
        public BasePfeContext()
        {
        }

        public BasePfeContext(DbContextOptions<BasePfeContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Camion> Camion { get; set; }
        public virtual DbSet<Chauffeur> Chauffeur { get; set; }
        public virtual DbSet<Client> Client { get; set; }
        public virtual DbSet<DemandeDevis> DemandeDevis { get; set; }
        public virtual DbSet<DemandeLivraison> DemandeLivraison { get; set; }
        public virtual DbSet<EtatDemandeDevis> EtatDemandeDevis { get; set; }
        public virtual DbSet<EtatDemandeLivraison> EtatDemandeLivraison { get; set; }
        public virtual DbSet<EtatOffre> EtatOffre { get; set; }
        public virtual DbSet<FactureClient> FactureClient { get; set; }
        public virtual DbSet<FactureTransporteur> FactureTransporteur { get; set; }
        public virtual DbSet<FileDemandeLivraison> FileDemandeLivraison { get; set; }
        public virtual DbSet<FileFactureTransporteur> FileFactureTransporteur { get; set; }
        public virtual DbSet<FileOffre> FileOffre { get; set; }
        public virtual DbSet<Intermediaire> Intermediaire { get; set; }
        public virtual DbSet<Itineraire> Itineraire { get; set; }
        public virtual DbSet<Offre> Offre { get; set; }
        public virtual DbSet<Permission> Permission { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<RolePermission> RolePermission { get; set; }
        public virtual DbSet<Societe> Societe { get; set; }
        public virtual DbSet<Trajet> Trajet { get; set; }
        public virtual DbSet<Transporteur> Transporteur { get; set; }
        public virtual DbSet<TypeCamion> TypeCamion { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Ville> Ville { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=BasePfe;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Camion>(entity =>
            {
                entity.HasKey(e => e.Idcamion);

                entity.Property(e => e.Codevehicule)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdchauffeurNavigation)
                    .WithMany(p => p.Camion)
                    .HasForeignKey(d => d.Idchauffeur)
                    .HasConstraintName("FK_Camion_Chauffeur");

                entity.HasOne(d => d.IdtransporteurNavigation)
                    .WithMany(p => p.Camion)
                    .HasForeignKey(d => d.Idtransporteur)
                    .HasConstraintName("FK_Camion_Transporteur");

                entity.HasOne(d => d.IdtypeNavigation)
                    .WithMany(p => p.Camion)
                    .HasForeignKey(d => d.Idtype)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Camion_TypeCamion");
            });

            modelBuilder.Entity<Chauffeur>(entity =>
            {
                entity.HasKey(e => e.Idchauffeur);

                entity.Property(e => e.Cinchauffeur)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IduserNavigation)
                    .WithMany(p => p.Chauffeur)
                    .HasForeignKey(d => d.Iduser)
                    .HasConstraintName("FK_Chauffeur_Users");
            });

            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Idclient);

                entity.HasOne(d => d.IduserNavigation)
                    .WithMany(p => p.Client)
                    .HasForeignKey(d => d.Iduser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Client_Users");
            });

            modelBuilder.Entity<DemandeDevis>(entity =>
            {
                entity.HasKey(e => e.IdDemandeDevis);

                entity.Property(e => e.DateEnvoit).HasColumnType("date");

                entity.HasOne(d => d.IdDemandeNavigation)
                    .WithMany(p => p.DemandeDevis)
                    .HasForeignKey(d => d.IdDemande)
                    .HasConstraintName("FK_DemandeDevis_DemandeLivraison");

                entity.HasOne(d => d.IdEtatNavigation)
                    .WithMany(p => p.DemandeDevis)
                    .HasForeignKey(d => d.IdEtat)
                    .HasConstraintName("FK_DemandeDevis_EtatDemandeDevis");

                entity.HasOne(d => d.IdIntermediaireNavigation)
                    .WithMany(p => p.DemandeDevis)
                    .HasForeignKey(d => d.IdIntermediaire)
                    .HasConstraintName("FK_DemandeDevis_Intermediaire");

                entity.HasOne(d => d.IdTransporteurNavigation)
                    .WithMany(p => p.DemandeDevis)
                    .HasForeignKey(d => d.IdTransporteur)
                    .HasConstraintName("FK_DemandeDevis_Transporteur");
            });

            modelBuilder.Entity<DemandeLivraison>(entity =>
            {
                entity.HasKey(e => e.IdDemande);

                entity.Property(e => e.Adressarrive)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Adressdepart)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Datecreation).HasColumnType("date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("text");

                entity.HasOne(d => d.IdEtatdemandeNavigation)
                    .WithMany(p => p.DemandeLivraison)
                    .HasForeignKey(d => d.IdEtatdemande)
                    .HasConstraintName("FK_DemandeLivraison_EtatDemandeLivraison");

                entity.HasOne(d => d.IdclientNavigation)
                    .WithMany(p => p.DemandeLivraison)
                    .HasForeignKey(d => d.Idclient)
                    .HasConstraintName("FK_DemandeLivraison_Client");
            });

            modelBuilder.Entity<EtatDemandeDevis>(entity =>
            {
                entity.HasKey(e => e.IdEtat);

                entity.Property(e => e.IdEtat).HasColumnName("idEtat");

                entity.Property(e => e.Etat)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EtatDemandeLivraison>(entity =>
            {
                entity.HasKey(e => e.IdEtatDemande);

                entity.Property(e => e.EtatDemande)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EtatOffre>(entity =>
            {
                entity.HasKey(e => e.IdEtat);

                entity.Property(e => e.Etat)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<FactureClient>(entity =>
            {
                entity.HasKey(e => e.IdFactClient);

                entity.Property(e => e.EtatFacture)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FactureFile)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PayementFile)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdDemandeLivraisonNavigation)
                    .WithMany(p => p.FactureClient)
                    .HasForeignKey(d => d.IdDemandeLivraison)
                    .HasConstraintName("FK_FactureClient_DemandeLivraison");
            });

            modelBuilder.Entity<FactureTransporteur>(entity =>
            {
                entity.HasKey(e => e.IdFactTransporteur);

                entity.Property(e => e.EtatFacture)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FactureFile)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PayementFile)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdOffreNavigation)
                    .WithMany(p => p.FactureTransporteur)
                    .HasForeignKey(d => d.IdOffre)
                    .HasConstraintName("FK_FactureTransporteur_Offre");
            });

            modelBuilder.Entity<FileDemandeLivraison>(entity =>
            {
                entity.HasKey(e => e.IdFile);

                entity.Property(e => e.IdFile).HasColumnName("idFile");

                entity.Property(e => e.NomFile)
                    .IsRequired()
                    .HasColumnName("nomFile")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdDemandeNavigation)
                    .WithMany(p => p.FileDemandeLivraison)
                    .HasForeignKey(d => d.IdDemande)
                    .HasConstraintName("FK_FileDemandeLivraison_DemandeLivraison");
            });

            modelBuilder.Entity<FileFactureTransporteur>(entity =>
            {
                entity.HasKey(e => e.IdFile);

                entity.Property(e => e.IdFile).HasColumnName("idFile");

                entity.Property(e => e.NomFile)
                    .IsRequired()
                    .HasColumnName("nomFile")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdFactTransporteurNavigation)
                    .WithMany(p => p.FileFactureTransporteur)
                    .HasForeignKey(d => d.IdFactTransporteur)
                    .HasConstraintName("FK_FileFactureTransporteur_FactureTransporteur");
            });

            modelBuilder.Entity<FileOffre>(entity =>
            {
                entity.HasKey(e => e.IdFile)
                    .HasName("PK_FileLivraison");

                entity.Property(e => e.NomFile)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdOffreNavigation)
                    .WithMany(p => p.FileOffre)
                    .HasForeignKey(d => d.IdOffre)
                    .HasConstraintName("FK_FileLivraison_Offre");
            });

            modelBuilder.Entity<Intermediaire>(entity =>
            {
                entity.HasKey(e => e.IdIntermediaire);

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.Intermediaire)
                    .HasForeignKey(d => d.IdRole)
                    .HasConstraintName("FK_Intermediaire_Role");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Intermediaire)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK_Intermediaire_Users");
            });

            modelBuilder.Entity<Itineraire>(entity =>
            {
                entity.HasKey(e => e.IdItineraire)
                    .HasName("PK_Itineraire2");
            });

            modelBuilder.Entity<Offre>(entity =>
            {
                entity.HasKey(e => e.IdOffre)
                    .HasName("PK_OffreRecu");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("date");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("text");

                entity.HasOne(d => d.IdDemandeNavigation)
                    .WithMany(p => p.Offre)
                    .HasForeignKey(d => d.IdDemande)
                    .HasConstraintName("FK_Offre_DemandeLivraison");

                entity.HasOne(d => d.IdEtatNavigation)
                    .WithMany(p => p.Offre)
                    .HasForeignKey(d => d.IdEtat)
                    .HasConstraintName("FK_Offre_EtatOffre");

                entity.HasOne(d => d.IdTransporteurNavigation)
                    .WithMany(p => p.Offre)
                    .HasForeignKey(d => d.IdTransporteur)
                    .HasConstraintName("FK_Offre_Transporteur");
            });

            modelBuilder.Entity<Permission>(entity =>
            {
                entity.HasKey(e => e.IdPermission);

                entity.Property(e => e.Permission1)
                    .IsRequired()
                    .HasColumnName("Permission")
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole);

                entity.Property(e => e.Role1)
                    .IsRequired()
                    .HasColumnName("Role")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RolePermission>(entity =>
            {
                entity.HasKey(e => e.IdRolePermission);

                entity.HasOne(d => d.IdPermisionNavigation)
                    .WithMany(p => p.RolePermission)
                    .HasForeignKey(d => d.IdPermision)
                    .HasConstraintName("FK_RolePermission_Permission");

                entity.HasOne(d => d.IdRoleNavigation)
                    .WithMany(p => p.RolePermission)
                    .HasForeignKey(d => d.IdRole)
                    .HasConstraintName("FK_RolePermission_Role");
            });

            modelBuilder.Entity<Societe>(entity =>
            {
                entity.HasKey(e => e.IdSociete);

                entity.Property(e => e.Adress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Trajet>(entity =>
            {
                entity.HasKey(e => e.IdTrajet);

                entity.Property(e => e.Date).HasColumnType("date");

                entity.HasOne(d => d.IdCamionNavigation)
                    .WithMany(p => p.Trajet)
                    .HasForeignKey(d => d.IdCamion)
                    .HasConstraintName("FK_Trajet_Camion");

                entity.HasOne(d => d.IdVille1Navigation)
                    .WithMany(p => p.TrajetIdVille1Navigation)
                    .HasForeignKey(d => d.IdVille1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trajet_Ville");

                entity.HasOne(d => d.IdVille2Navigation)
                    .WithMany(p => p.TrajetIdVille2Navigation)
                    .HasForeignKey(d => d.IdVille2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Trajet_Ville1");
            });

            modelBuilder.Entity<Transporteur>(entity =>
            {
                entity.HasKey(e => e.IdTransporteur);

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Transporteur)
                    .HasForeignKey(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Transporteur_Users");
            });

            modelBuilder.Entity<TypeCamion>(entity =>
            {
                entity.HasKey(e => e.IdType);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.IdUser);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Motdepasse)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Nom)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Prenom)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.SocieteNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Societe)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Users_Societe");
            });

            modelBuilder.Entity<Ville>(entity =>
            {
                entity.HasKey(e => e.IdVille);

                entity.Property(e => e.NomVille)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.IdIntermediaireNavigation)
                    .WithMany(p => p.Ville)
                    .HasForeignKey(d => d.IdIntermediaire)
                    .HasConstraintName("FK_Ville_Intermediaire");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
