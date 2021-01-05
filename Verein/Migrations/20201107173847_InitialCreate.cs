using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Verein.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Arbeitseinsaetze",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Datum = table.Column<DateTime>(nullable: false),
                    Titel = table.Column<string>(nullable: false),
                    Taetigkeit = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arbeitseinsaetze", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FullName = table.Column<string>(nullable: true),
                    Approved = table.Column<bool>(nullable: false),
                    Rolle = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Familien",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Familien", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Inventar",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Ort = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kurse",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Titel = table.Column<string>(nullable: false),
                    Beschreibung = table.Column<string>(nullable: true),
                    Startdatum = table.Column<DateTime>(nullable: true),
                    Enddatum = table.Column<DateTime>(nullable: true),
                    Von = table.Column<DateTime>(nullable: false),
                    Bis = table.Column<DateTime>(nullable: false),
                    Wochentag = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kurse", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stammdaten",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stammdaten", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tarife",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(nullable: true),
                    Fee = table.Column<decimal>(type: "decimal(18, 2)", nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tarife", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zahlungsinformationen",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BankName = table.Column<string>(nullable: false),
                    KontoInhaber = table.Column<string>(nullable: false),
                    Iban = table.Column<string>(nullable: false),
                    Bic = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zahlungsinformationen", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
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
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
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
                name: "Mitglieder",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MitgliedsNummer = table.Column<string>(nullable: false),
                    SwhvMitgliedsNummer = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Vorname = table.Column<string>(nullable: false),
                    Typ = table.Column<int>(nullable: false),
                    Passiv = table.Column<bool>(nullable: false),
                    Familienmitgliedschaft = table.Column<bool>(nullable: false),
                    Geburtstag = table.Column<DateTime>(nullable: false),
                    Telefonnummer = table.Column<string>(nullable: true),
                    HandyNummer = table.Column<string>(nullable: true),
                    EMail = table.Column<string>(nullable: true),
                    Strasse = table.Column<string>(nullable: false),
                    Hausnummer = table.Column<string>(nullable: false),
                    Postleitzahl = table.Column<string>(nullable: false),
                    Ort = table.Column<string>(nullable: false),
                    Eintrittsdatum = table.Column<DateTime>(nullable: false),
                    Austrittsdatum = table.Column<DateTime>(nullable: true),
                    Entfernung = table.Column<double>(nullable: false),
                    Bemerkung = table.Column<string>(nullable: true),
                    ZahlungsInfoId = table.Column<int>(nullable: true),
                    FamilieId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mitglieder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Mitglieder_Familien_FamilieId",
                        column: x => x.FamilieId,
                        principalTable: "Familien",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Mitglieder_Zahlungsinformationen_ZahlungsInfoId",
                        column: x => x.ZahlungsInfoId,
                        principalTable: "Zahlungsinformationen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Helfer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Dauer = table.Column<DateTime>(nullable: false),
                    ArbeitseinsatzId = table.Column<int>(nullable: true),
                    TeilnehmerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Helfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Helfer_Arbeitseinsaetze_ArbeitseinsatzId",
                        column: x => x.ArbeitseinsatzId,
                        principalTable: "Arbeitseinsaetze",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Helfer_Mitglieder_TeilnehmerId",
                        column: x => x.TeilnehmerId,
                        principalTable: "Mitglieder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hunde",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: false),
                    Zwingername = table.Column<string>(nullable: true),
                    Rasse = table.Column<string>(nullable: false),
                    Geburtsdatum = table.Column<DateTime>(nullable: true),
                    ChipNummer = table.Column<string>(nullable: true),
                    Geimpft = table.Column<bool>(nullable: false),
                    Versichert = table.Column<bool>(nullable: false),
                    BesitzerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hunde", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hunde_Mitglieder_BesitzerId",
                        column: x => x.BesitzerId,
                        principalTable: "Mitglieder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "KursTeilnehmer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TeilnehmerId = table.Column<int>(nullable: true),
                    KurseId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KursTeilnehmer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KursTeilnehmer_Kurse_KurseId",
                        column: x => x.KurseId,
                        principalTable: "Kurse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KursTeilnehmer_Mitglieder_TeilnehmerId",
                        column: x => x.TeilnehmerId,
                        principalTable: "Mitglieder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Trainer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KursTrainerId = table.Column<int>(nullable: true),
                    KurseId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trainer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trainer_Mitglieder_KursTrainerId",
                        column: x => x.KursTrainerId,
                        principalTable: "Mitglieder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Trainer_Kurse_KurseId",
                        column: x => x.KurseId,
                        principalTable: "Kurse",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

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
                name: "IX_Helfer_ArbeitseinsatzId",
                table: "Helfer",
                column: "ArbeitseinsatzId");

            migrationBuilder.CreateIndex(
                name: "IX_Helfer_TeilnehmerId",
                table: "Helfer",
                column: "TeilnehmerId");

            migrationBuilder.CreateIndex(
                name: "IX_Hunde_BesitzerId",
                table: "Hunde",
                column: "BesitzerId");

            migrationBuilder.CreateIndex(
                name: "IX_KursTeilnehmer_KurseId",
                table: "KursTeilnehmer",
                column: "KurseId");

            migrationBuilder.CreateIndex(
                name: "IX_KursTeilnehmer_TeilnehmerId",
                table: "KursTeilnehmer",
                column: "TeilnehmerId");

            migrationBuilder.CreateIndex(
                name: "IX_Mitglieder_FamilieId",
                table: "Mitglieder",
                column: "FamilieId");

            migrationBuilder.CreateIndex(
                name: "IX_Mitglieder_ZahlungsInfoId",
                table: "Mitglieder",
                column: "ZahlungsInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainer_KursTrainerId",
                table: "Trainer",
                column: "KursTrainerId");

            migrationBuilder.CreateIndex(
                name: "IX_Trainer_KurseId",
                table: "Trainer",
                column: "KurseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "Helfer");

            migrationBuilder.DropTable(
                name: "Hunde");

            migrationBuilder.DropTable(
                name: "Inventar");

            migrationBuilder.DropTable(
                name: "KursTeilnehmer");

            migrationBuilder.DropTable(
                name: "Stammdaten");

            migrationBuilder.DropTable(
                name: "Tarife");

            migrationBuilder.DropTable(
                name: "Trainer");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Arbeitseinsaetze");

            migrationBuilder.DropTable(
                name: "Mitglieder");

            migrationBuilder.DropTable(
                name: "Kurse");

            migrationBuilder.DropTable(
                name: "Familien");

            migrationBuilder.DropTable(
                name: "Zahlungsinformationen");
        }
    }
}
