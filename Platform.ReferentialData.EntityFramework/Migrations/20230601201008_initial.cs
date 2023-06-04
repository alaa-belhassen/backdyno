using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Platform.ReferentialData.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    walletPublicKey = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payement",
                columns: table => new
                {
                    idPayment = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    dayOfwork = table.Column<string>(type: "text", nullable: false),
                    idsociété = table.Column<string>(type: "text", nullable: false),
                    idEmployer = table.Column<string>(type: "text", nullable: false),
                    montant = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payement", x => x.idPayment);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokenTable",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    JwtId = table.Column<string>(type: "text", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokenTable", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    IdTicket = table.Column<string>(type: "text", nullable: false),
                    nameTicket = table.Column<string>(type: "text", nullable: false),
                    prixTicket = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<bool>(type: "boolean", nullable: false),
                    idEmployer = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.IdTicket);
                });

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    idaccount = table.Column<string>(type: "text", nullable: false),
                    secret = table.Column<string>(type: "text", nullable: false),
                    iduser = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.idaccount);
                    table.ForeignKey(
                        name: "FK_accounts_AspNetUsers_iduser",
                        column: x => x.iduser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    moreInfoId = table.Column<string>(type: "text", nullable: false),
                    NumTel = table.Column<string>(type: "text", nullable: false),
                    balance = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.moreInfoId);
                    table.ForeignKey(
                        name: "Fk_USER_Employee",
                        column: x => x.moreInfoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Employer",
                columns: table => new
                {
                    moreInfoId = table.Column<string>(type: "text", nullable: false),
                    matriculeFiscale = table.Column<string>(type: "text", nullable: false),
                    codeTVA = table.Column<string>(type: "text", nullable: false),
                    AdresseEntreprise = table.Column<string>(type: "text", nullable: false),
                    EmailRH = table.Column<string>(type: "text", nullable: false),
                    NumTelRH = table.Column<string>(type: "text", nullable: false),
                    PaymentMethode = table.Column<string>(type: "text", nullable: false),
                    numeroTelEntreprise = table.Column<string>(type: "text", nullable: false),
                    adresseFacturation = table.Column<string>(type: "text", nullable: false),
                    Blocked = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employer", x => x.moreInfoId);
                    table.ForeignKey(
                        name: "Fk_USER_MoreInfo",
                        column: x => x.moreInfoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shopowner",
                columns: table => new
                {
                    moreInfoId = table.Column<string>(type: "text", nullable: false),
                    codeTVA = table.Column<string>(type: "text", nullable: false),
                    Adresse = table.Column<string>(type: "text", nullable: false),
                    NumTel = table.Column<string>(type: "text", nullable: false),
                    PaymentMethode = table.Column<string>(type: "text", nullable: false),
                    adresseFacturation = table.Column<string>(type: "text", nullable: false),
                    matriculeFiscale = table.Column<string>(type: "text", nullable: false),
                    commission = table.Column<string>(type: "text", nullable: false),
                    Blocked = table.Column<string>(type: "text", nullable: false),
                    delaiPayement = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shopowner", x => x.moreInfoId);
                    table.ForeignKey(
                        name: "Fk_USER_ShopMoreInfo",
                        column: x => x.moreInfoId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "superuser",
                columns: table => new
                {
                    Idsuperuser = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_superuser", x => x.Idsuperuser);
                    table.ForeignKey(
                        name: "FK_superuser_AspNetUsers_Idsuperuser",
                        column: x => x.Idsuperuser,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "categorie",
                columns: table => new
                {
                    IdCategorie = table.Column<string>(type: "text", nullable: false),
                    NameCateforie = table.Column<string>(type: "text", nullable: false),
                    idticket = table.Column<string>(type: "text", nullable: false),
                    idEmployer = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorie", x => x.IdCategorie);
                    table.ForeignKey(
                        name: "FK_categorie_Ticket_idticket",
                        column: x => x.idticket,
                        principalTable: "Ticket",
                        principalColumn: "IdTicket",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    idEmployer = table.Column<string>(type: "text", nullable: false),
                    MoreInfoEmployermoreInfoId = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoles_Employer_MoreInfoEmployermoreInfoId",
                        column: x => x.MoreInfoEmployermoreInfoId,
                        principalTable: "Employer",
                        principalColumn: "moreInfoId");
                });

            migrationBuilder.CreateTable(
                name: "DemandeTransaction",
                columns: table => new
                {
                    IdDemandeTransaction = table.Column<string>(type: "text", nullable: false),
                    IdEmployer = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    amount = table.Column<string>(type: "text", nullable: false),
                    etat = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DemandeTransaction", x => x.IdDemandeTransaction);
                    table.ForeignKey(
                        name: "FK_DemandeTransaction_Employer_IdEmployer",
                        column: x => x.IdEmployer,
                        principalTable: "Employer",
                        principalColumn: "moreInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    IdTransaction = table.Column<string>(type: "text", nullable: false),
                    IdVerificateur = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    montant = table.Column<float>(type: "real", nullable: false),
                    etat = table.Column<string>(type: "text", nullable: false),
                    IdEmployer = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.IdTransaction);
                    table.ForeignKey(
                        name: "FK_transactions_Employer_IdEmployer",
                        column: x => x.IdEmployer,
                        principalTable: "Employer",
                        principalColumn: "moreInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "demandePayement",
                columns: table => new
                {
                    IdDemandePayement = table.Column<string>(type: "text", nullable: false),
                    IdShopowner = table.Column<string>(type: "text", nullable: false),
                    date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    amount = table.Column<string>(type: "text", nullable: false),
                    etat = table.Column<string>(type: "text", nullable: false),
                    IdValidateur = table.Column<string>(type: "text", nullable: false),
                    walletValidateur = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_demandePayement", x => x.IdDemandePayement);
                    table.ForeignKey(
                        name: "FK_demandePayement_Shopowner_IdShopowner",
                        column: x => x.IdShopowner,
                        principalTable: "Shopowner",
                        principalColumn: "moreInfoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employement",
                columns: table => new
                {
                    IdEmployee = table.Column<string>(type: "text", nullable: false),
                    IdEmployer = table.Column<string>(type: "text", nullable: false),
                    IdCategorie = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employement", x => new { x.IdEmployee, x.IdEmployer });
                    table.ForeignKey(
                        name: "FK_Employement_Employee_IdEmployee",
                        column: x => x.IdEmployee,
                        principalTable: "Employee",
                        principalColumn: "moreInfoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employement_Employer_IdEmployer",
                        column: x => x.IdEmployer,
                        principalTable: "Employer",
                        principalColumn: "moreInfoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Employement_categorie_IdCategorie",
                        column: x => x.IdCategorie,
                        principalTable: "categorie",
                        principalColumn: "IdCategorie",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_iduser",
                table: "accounts",
                column: "iduser",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoles_MoreInfoEmployermoreInfoId",
                table: "AspNetRoles",
                column: "MoreInfoEmployermoreInfoId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_categorie_idticket",
                table: "categorie",
                column: "idticket");

            migrationBuilder.CreateIndex(
                name: "IX_demandePayement_IdShopowner",
                table: "demandePayement",
                column: "IdShopowner");

            migrationBuilder.CreateIndex(
                name: "IX_DemandeTransaction_IdEmployer",
                table: "DemandeTransaction",
                column: "IdEmployer");

            migrationBuilder.CreateIndex(
                name: "IX_Employement_IdCategorie",
                table: "Employement",
                column: "IdCategorie");

            migrationBuilder.CreateIndex(
                name: "IX_Employement_IdEmployer",
                table: "Employement",
                column: "IdEmployer");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_IdEmployer",
                table: "transactions",
                column: "IdEmployer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "demandePayement");

            migrationBuilder.DropTable(
                name: "DemandeTransaction");

            migrationBuilder.DropTable(
                name: "Employement");

            migrationBuilder.DropTable(
                name: "Payement");

            migrationBuilder.DropTable(
                name: "RefreshTokenTable");

            migrationBuilder.DropTable(
                name: "superuser");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Shopowner");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "categorie");

            migrationBuilder.DropTable(
                name: "Employer");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
