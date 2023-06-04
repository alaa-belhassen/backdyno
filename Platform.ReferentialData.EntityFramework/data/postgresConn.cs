using login.models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Platform.ReferentialData.DataModel.Authentification;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using WebApplication1.models;

namespace  login.data.PostgresConn
{
    public class Postgres : IdentityDbContext<employer3, AspRoles,string>
    {

        public virtual DbSet<payement> Payement { get; set; }
        public virtual DbSet<MoreInfoEmployer> Employer {get;set;}
        public virtual DbSet<ShopOwner> Shopowner { get; set; }
        public virtual DbSet<superuser> superuser { get; set; }
        public virtual DbSet<accounts> accounts { get; set; }

        public virtual DbSet<RefreshTokenModel> RefreshTokenTable {get;set;}
        public virtual DbSet<Employee> Employee{ get; set;}
        public virtual DbSet<Employment> Employement { get; set; }
        public virtual DbSet<Categorie> categorie { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
        public virtual DbSet<demandeTransaction> DemandeTransaction { get; set; }
        public virtual DbSet<demandePayement> demandePayement { get; set; }
        public virtual DbSet<TransactionTerminer> transactions { get; set; }


        public Postgres(DbContextOptions<Postgres> options) : base(options)
        {
             
        }

        public Postgres()
        {
        }

        protected override void OnModelCreating(ModelBuilder model){
            base.OnModelCreating(model);
            model.Entity<employer3>(entity => 
                entity.HasOne(E=> E.moreInfo)
                .WithOne(M => M.employer)
                .HasForeignKey<MoreInfoEmployer>(m => m.moreInfoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("Fk_USER_MoreInfo")
            );
            model.Entity<employer3>(entity =>
              entity.HasOne(E => E.ShopOwnerMoreInfo)
              .WithOne(M => M.employer)
              .HasForeignKey<ShopOwner>(m => m.moreInfoId)
              .OnDelete(DeleteBehavior.Restrict)
              .HasConstraintName("Fk_USER_ShopMoreInfo")
          );
            model.Entity<employer3>(entity =>
           entity.HasOne(E => E.EmployeeId)
           .WithOne(M => M.user)
           .HasForeignKey<Employee>(m => m.moreInfoId)
           .OnDelete(DeleteBehavior.Restrict)
           .HasConstraintName("Fk_USER_Employee")
       );

            model.Entity<employer3>()
                .HasOne(e => e.superuser)
                .WithOne(M => M.user)
                .HasForeignKey<superuser>(e => e.Idsuperuser)
                .IsRequired();

            model.Entity<employer3>()
                .HasOne(e => e.account)
                .WithOne(M => M.user)
                .HasForeignKey<accounts>(e => e.iduser);



            model.Entity<Employment>()
            .HasKey(e => new { e.IdEmployee, e.IdEmployer});

            model.Entity<Employment>()
               .HasOne(s => s.employee)
               .WithMany(c => c.employment)
               .HasForeignKey(s => s.IdEmployee);
               
            model.Entity<Employment>()
               .HasOne(s => s.employer)
               .WithMany(c => c.employment)
               .HasForeignKey(s => s.IdEmployer);

            model.Entity<Employment>()
               .HasOne(s => s.Categorie)
               .WithMany(c => c.Employment)
               .HasForeignKey(s => s.IdCategorie);
            model.Entity<Categorie>()
               .HasOne(s => s.ticket)
               .WithMany(c => c.categorie)
               .HasForeignKey(s => s.idticket);

            model.Entity<demandeTransaction>()
                .HasOne(e => e.employer)
                .WithMany(e => e.demandeTransaction)
                .HasForeignKey(e => e.IdEmployer)
                .IsRequired();

            model.Entity<demandePayement>()
           .HasOne(e => e.shopowner)
           .WithMany(e => e.demandePayement)
           .HasForeignKey(e => e.IdShopowner)
           .IsRequired();

            model.Entity<TransactionTerminer>()
                .HasOne(e => e.employer)
                .WithMany(e => e.Transactions)
                .HasForeignKey(e => e.IdEmployer)
                .IsRequired();

           
        }

      
    }
}