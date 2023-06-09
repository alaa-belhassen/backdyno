﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using login.data.PostgresConn;

#nullable disable

namespace Platform.ReferentialData.EntityFramework.Migrations
{
    [DbContext(typeof(Postgres))]
    [Migration("20230601201008_initial")]
    partial class initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.AspRoles", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("MoreInfoEmployermoreInfoId")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("idEmployer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("MoreInfoEmployermoreInfoId");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.Categorie", b =>
                {
                    b.Property<string>("IdCategorie")
                        .HasColumnType("text");

                    b.Property<string>("NameCateforie")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("idEmployer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("idticket")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("IdCategorie");

                    b.HasIndex("idticket");

                    b.ToTable("categorie");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.Employee", b =>
                {
                    b.Property<string>("moreInfoId")
                        .HasColumnType("text");

                    b.Property<string>("NumTel")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<double>("balance")
                        .HasColumnType("double precision");

                    b.HasKey("moreInfoId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.Employment", b =>
                {
                    b.Property<string>("IdEmployee")
                        .HasColumnType("text");

                    b.Property<string>("IdEmployer")
                        .HasColumnType("text");

                    b.Property<string>("IdCategorie")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("IdEmployee", "IdEmployer");

                    b.HasIndex("IdCategorie");

                    b.HasIndex("IdEmployer");

                    b.ToTable("Employement");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.Ticket", b =>
                {
                    b.Property<string>("IdTicket")
                        .HasColumnType("text");

                    b.Property<string>("idEmployer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("nameTicket")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("prixTicket")
                        .HasColumnType("integer");

                    b.Property<bool>("status")
                        .HasColumnType("boolean");

                    b.HasKey("IdTicket");

                    b.ToTable("Ticket");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.TransactionTerminer", b =>
                {
                    b.Property<string>("IdTransaction")
                        .HasColumnType("text");

                    b.Property<string>("IdEmployer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IdVerificateur")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("etat")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<float>("montant")
                        .HasColumnType("real");

                    b.HasKey("IdTransaction");

                    b.HasIndex("IdEmployer");

                    b.ToTable("transactions");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.accounts", b =>
                {
                    b.Property<string>("idaccount")
                        .HasColumnType("text");

                    b.Property<string>("iduser")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("secret")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("idaccount");

                    b.HasIndex("iduser")
                        .IsUnique();

                    b.ToTable("accounts");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.demandePayement", b =>
                {
                    b.Property<string>("IdDemandePayement")
                        .HasColumnType("text");

                    b.Property<string>("IdShopowner")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IdValidateur")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("amount")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("etat")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("walletValidateur")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("IdDemandePayement");

                    b.HasIndex("IdShopowner");

                    b.ToTable("demandePayement");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.demandeTransaction", b =>
                {
                    b.Property<string>("IdDemandeTransaction")
                        .HasColumnType("text");

                    b.Property<string>("IdEmployer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("amount")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("etat")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("IdDemandeTransaction");

                    b.HasIndex("IdEmployer");

                    b.ToTable("DemandeTransaction");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.payement", b =>
                {
                    b.Property<string>("idPayment")
                        .HasColumnType("text");

                    b.Property<DateTime>("date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("dayOfwork")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("idEmployer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("idsociété")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("montant")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("idPayment");

                    b.ToTable("Payement");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.superuser", b =>
                {
                    b.Property<string>("Idsuperuser")
                        .HasColumnType("text");

                    b.HasKey("Idsuperuser");

                    b.ToTable("superuser");
                });

            modelBuilder.Entity("WebApplication1.models.ShopOwner", b =>
                {
                    b.Property<string>("moreInfoId")
                        .HasColumnType("text");

                    b.Property<string>("Adresse")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Blocked")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NumTel")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PaymentMethode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("adresseFacturation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("codeTVA")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("commission")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("delaiPayement")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("matriculeFiscale")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("moreInfoId");

                    b.ToTable("Shopowner");
                });

            modelBuilder.Entity("login.models.MoreInfoEmployer", b =>
                {
                    b.Property<string>("moreInfoId")
                        .HasColumnType("text");

                    b.Property<string>("AdresseEntreprise")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Blocked")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EmailRH")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("NumTelRH")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PaymentMethode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("adresseFacturation")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("codeTVA")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("matriculeFiscale")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("numeroTelEntreprise")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("moreInfoId");

                    b.ToTable("Employer");
                });

            modelBuilder.Entity("login.models.RefreshTokenModel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateTime>("AddedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsRevoked")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("boolean");

                    b.Property<string>("JwtId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokenTable");
                });

            modelBuilder.Entity("login.models.employer3", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("walletPublicKey")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Platform.ReferentialData.DataModel.Authentification.AspRoles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("login.models.employer3", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("login.models.employer3", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Platform.ReferentialData.DataModel.Authentification.AspRoles", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("login.models.employer3", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("login.models.employer3", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.AspRoles", b =>
                {
                    b.HasOne("login.models.MoreInfoEmployer", null)
                        .WithMany("hasRole")
                        .HasForeignKey("MoreInfoEmployermoreInfoId");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.Categorie", b =>
                {
                    b.HasOne("Platform.ReferentialData.DataModel.Authentification.Ticket", "ticket")
                        .WithMany("categorie")
                        .HasForeignKey("idticket")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ticket");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.Employee", b =>
                {
                    b.HasOne("login.models.employer3", "user")
                        .WithOne("EmployeeId")
                        .HasForeignKey("Platform.ReferentialData.DataModel.Authentification.Employee", "moreInfoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("Fk_USER_Employee");

                    b.Navigation("user");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.Employment", b =>
                {
                    b.HasOne("Platform.ReferentialData.DataModel.Authentification.Categorie", "Categorie")
                        .WithMany("Employment")
                        .HasForeignKey("IdCategorie")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Platform.ReferentialData.DataModel.Authentification.Employee", "employee")
                        .WithMany("employment")
                        .HasForeignKey("IdEmployee")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("login.models.MoreInfoEmployer", "employer")
                        .WithMany("employment")
                        .HasForeignKey("IdEmployer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Categorie");

                    b.Navigation("employee");

                    b.Navigation("employer");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.TransactionTerminer", b =>
                {
                    b.HasOne("login.models.MoreInfoEmployer", "employer")
                        .WithMany("Transactions")
                        .HasForeignKey("IdEmployer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("employer");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.accounts", b =>
                {
                    b.HasOne("login.models.employer3", "user")
                        .WithOne("account")
                        .HasForeignKey("Platform.ReferentialData.DataModel.Authentification.accounts", "iduser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.demandePayement", b =>
                {
                    b.HasOne("WebApplication1.models.ShopOwner", "shopowner")
                        .WithMany("demandePayement")
                        .HasForeignKey("IdShopowner")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("shopowner");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.demandeTransaction", b =>
                {
                    b.HasOne("login.models.MoreInfoEmployer", "employer")
                        .WithMany("demandeTransaction")
                        .HasForeignKey("IdEmployer")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("employer");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.superuser", b =>
                {
                    b.HasOne("login.models.employer3", "user")
                        .WithOne("superuser")
                        .HasForeignKey("Platform.ReferentialData.DataModel.Authentification.superuser", "Idsuperuser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("user");
                });

            modelBuilder.Entity("WebApplication1.models.ShopOwner", b =>
                {
                    b.HasOne("login.models.employer3", "employer")
                        .WithOne("ShopOwnerMoreInfo")
                        .HasForeignKey("WebApplication1.models.ShopOwner", "moreInfoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("Fk_USER_ShopMoreInfo");

                    b.Navigation("employer");
                });

            modelBuilder.Entity("login.models.MoreInfoEmployer", b =>
                {
                    b.HasOne("login.models.employer3", "employer")
                        .WithOne("moreInfo")
                        .HasForeignKey("login.models.MoreInfoEmployer", "moreInfoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("Fk_USER_MoreInfo");

                    b.Navigation("employer");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.Categorie", b =>
                {
                    b.Navigation("Employment");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.Employee", b =>
                {
                    b.Navigation("employment");
                });

            modelBuilder.Entity("Platform.ReferentialData.DataModel.Authentification.Ticket", b =>
                {
                    b.Navigation("categorie");
                });

            modelBuilder.Entity("WebApplication1.models.ShopOwner", b =>
                {
                    b.Navigation("demandePayement");
                });

            modelBuilder.Entity("login.models.MoreInfoEmployer", b =>
                {
                    b.Navigation("Transactions");

                    b.Navigation("demandeTransaction");

                    b.Navigation("employment");

                    b.Navigation("hasRole");
                });

            modelBuilder.Entity("login.models.employer3", b =>
                {
                    b.Navigation("EmployeeId")
                        .IsRequired();

                    b.Navigation("ShopOwnerMoreInfo")
                        .IsRequired();

                    b.Navigation("account")
                        .IsRequired();

                    b.Navigation("moreInfo")
                        .IsRequired();

                    b.Navigation("superuser")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
