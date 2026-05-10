using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BioDomes.Infrastructures.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Consumptions",
                columns: table => new
                {
                    ConsumptionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumptions", x => x.ConsumptionId);
                });

            migrationBuilder.CreateTable(
                name: "Productions",
                columns: table => new
                {
                    ProductionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productions", x => x.ProductionId);
                });

            migrationBuilder.CreateTable(
                name: "ResearchOrganisms",
                columns: table => new
                {
                    ResearchOrganismId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResearchOrganisms", x => x.ResearchOrganismId);
                });

            migrationBuilder.CreateTable(
                name: "SpeciesRegimes",
                columns: table => new
                {
                    IdRegime = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesRegimes", x => x.IdRegime);
                });

            migrationBuilder.CreateTable(
                name: "SpeciesTypes",
                columns: table => new
                {
                    IdType = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpeciesTypes", x => x.IdType);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false),
                    ResearchOrganismId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_ResearchOrganisms_ResearchOrganismId",
                        column: x => x.ResearchOrganismId,
                        principalTable: "ResearchOrganisms",
                        principalColumn: "ResearchOrganismId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
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
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "Biodomes",
                columns: table => new
                {
                    idBiodomes = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    Humidity = table.Column<double>(type: "float", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Biodomes", x => x.idBiodomes);
                    table.ForeignKey(
                        name: "FK_Biodomes_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    IdEquipment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductionId = table.Column<int>(type: "int", nullable: false),
                    ConsumptionId = table.Column<int>(type: "int", nullable: true),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.IdEquipment);
                    table.ForeignKey(
                        name: "FK_Equipment_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Equipment_Consumptions_ConsumptionId",
                        column: x => x.ConsumptionId,
                        principalTable: "Consumptions",
                        principalColumn: "ConsumptionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Equipment_Productions_ProductionId",
                        column: x => x.ProductionId,
                        principalTable: "Productions",
                        principalColumn: "ProductionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Species",
                columns: table => new
                {
                    IdSpecies = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Avatar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    RegimId = table.Column<int>(type: "int", nullable: false),
                    Size = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    Visible = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Species", x => x.IdSpecies);
                    table.ForeignKey(
                        name: "FK_Species_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Species_SpeciesRegimes_RegimId",
                        column: x => x.RegimId,
                        principalTable: "SpeciesRegimes",
                        principalColumn: "IdRegime",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Species_SpeciesTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "SpeciesTypes",
                        principalColumn: "IdType",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BiodomeEquipment",
                columns: table => new
                {
                    BiodomeId = table.Column<int>(type: "int", nullable: false),
                    EquipmentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiodomeEquipment", x => new { x.BiodomeId, x.EquipmentId });
                    table.ForeignKey(
                        name: "FK_BiodomeEquipment_Biodomes_BiodomeId",
                        column: x => x.BiodomeId,
                        principalTable: "Biodomes",
                        principalColumn: "idBiodomes",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BiodomeEquipment_Equipment_EquipmentId",
                        column: x => x.EquipmentId,
                        principalTable: "Equipment",
                        principalColumn: "IdEquipment",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BiodomeHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BiodomeId = table.Column<int>(type: "int", nullable: false),
                    SpeciesId = table.Column<int>(type: "int", nullable: false),
                    Delta = table.Column<int>(type: "int", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiodomeHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BiodomeHistory_Biodomes_BiodomeId",
                        column: x => x.BiodomeId,
                        principalTable: "Biodomes",
                        principalColumn: "idBiodomes",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BiodomeHistory_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "IdSpecies",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BiodomeSpecies",
                columns: table => new
                {
                    BiodomeId = table.Column<int>(type: "int", nullable: false),
                    SpeciesId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BiodomeSpecies", x => new { x.BiodomeId, x.SpeciesId });
                    table.ForeignKey(
                        name: "FK_BiodomeSpecies_Biodomes_BiodomeId",
                        column: x => x.BiodomeId,
                        principalTable: "Biodomes",
                        principalColumn: "idBiodomes",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BiodomeSpecies_Species_SpeciesId",
                        column: x => x.SpeciesId,
                        principalTable: "Species",
                        principalColumn: "IdSpecies",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0", "0", "Admin", "ADMIN" },
                    { "1", "1", "Chercheur", "CHERCHEUR" }
                });

            migrationBuilder.InsertData(
                table: "Consumptions",
                columns: new[] { "ConsumptionId", "Name" },
                values: new object[,]
                {
                    { 1, "Hydrogène" },
                    { 2, "Electricité" },
                    { 3, "Essence" }
                });

            migrationBuilder.InsertData(
                table: "Productions",
                columns: new[] { "ProductionId", "Name" },
                values: new object[,]
                {
                    { 1, "Lumière" },
                    { 2, "Eau" },
                    { 3, "Azote" }
                });

            migrationBuilder.InsertData(
                table: "ResearchOrganisms",
                columns: new[] { "ResearchOrganismId", "Name" },
                values: new object[,]
                {
                    { "1", "ULiege" },
                    { "2", "UCLouvain" },
                    { "3", "HELMo" },
                    { "4", "Hec Liège" },
                    { "5", "Hénallux" },
                    { "6", "HEPL" }
                });

            migrationBuilder.InsertData(
                table: "SpeciesRegimes",
                columns: new[] { "IdRegime", "Name" },
                values: new object[,]
                {
                    { 1, "Photosynthèse" },
                    { 2, "Carnivore" },
                    { 3, "Omnivore" },
                    { 4, "Coprophage" },
                    { 5, "Herbivore" },
                    { 6, "Détritivores" },
                    { 7, "Hétérotrophie" },
                    { 8, "Coprophages" }
                });

            migrationBuilder.InsertData(
                table: "SpeciesTypes",
                columns: new[] { "IdType", "Name" },
                values: new object[,]
                {
                    { 1, "Mammiferes" },
                    { 2, "Oiseaux" },
                    { 3, "Poissons" },
                    { 4, "Reptiles" },
                    { 5, "Amphibiens" },
                    { 6, "Crustaces" },
                    { 7, "Arachnides" },
                    { 8, "Insectes" },
                    { 9, "Myriapodes" },
                    { 10, "Mollusques" },
                    { 11, "Echinodermes" },
                    { 12, "Anelides" },
                    { 13, "Cnidaires" },
                    { 14, "Plathelminthes" },
                    { 15, "Poriferes" },
                    { 16, "Plantes" },
                    { 17, "Mousses" },
                    { 18, "Algues" },
                    { 19, "Lichen" },
                    { 20, "Champignons" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Avatar", "ConcurrencyStamp", "DateOfBirth", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ResearchOrganismId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "1", 0, "3.png", "13c79dff-c3cb-4494-9a93-c4248965d536", new DateOnly(2001, 11, 1), "admin@company.com", true, false, null, "ADMIN@COMPANY.COM", "ADMIN", "AQAAAAIAAYagAAAAEIjkMsR3Bw14t+kPbIokHmlRQEUrqgLdxZK8sHFJOrBFZfL/9tvccchV+l016eEDng==", null, false, "1", "d308d05c-1cfb-417b-86bc-cc0401a9346e", false, "Admin" },
                    { "2", 0, "1.png", "184d84b6-935b-4d9b-ade1-2724d4edfde5", new DateOnly(2002, 3, 12), "research@uliege.be", true, false, null, "RESEARCH@ULIEGE.BE", "DARWIN", "AQAAAAIAAYagAAAAECGTpbOPHql+gF30ycOlr+XrR1yNSJ5czf785a8wNX5Zj1yestqt3hlBAWb6lGZf/g==", null, false, "1", "51dfb404-08c5-47f0-8eb2-8fd8885391f1", false, "Darwin" },
                    { "3", 0, "4.png", "9c0adac8-6b29-4473-9b25-c80b76951c06", new DateOnly(2001, 12, 21), "research@helmo.be", true, false, null, "RESEARCH@HELMO.BE", "JOHNDOE", "AQAAAAIAAYagAAAAELlGwZQm0LajUmq4HgMt193w3LGUxtMJfIugIO0ewCWqcTLJsrAoakX02fOonBnp1A==", null, false, "3", "73148a07-bb71-4fe3-8984-f112e28397df", false, "JohnDoe" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "0", "1" },
                    { "1", "2" },
                    { "1", "3" }
                });

            migrationBuilder.InsertData(
                table: "Biodomes",
                columns: new[] { "idBiodomes", "Avatar", "Humidity", "Name", "Slug", "Temperature", "userId" },
                values: new object[,]
                {
                    { 1, "foret-temperee.png", 70.0, "Forêt Tempérée", "foret-temperee", 18.0, "2" },
                    { 2, "foret-tropicale.png", 90.0, "Forêt Tropicale", "foret-tropicale", 28.0, "2" },
                    { 3, "savane.png", 40.0, "Savane", "savane", 32.0, "2" },
                    { 4, "prairie.png", 55.0, "Prairie Tempérée", "prairie-temperee", 20.0, "2" },
                    { 5, "labo.png", 18.0, "Laboratoire Contrôlé", "laboratoire-controle", 22.0, "1" }
                });

            migrationBuilder.InsertData(
                table: "Equipment",
                columns: new[] { "IdEquipment", "Avatar", "ConsumptionId", "Name", "ProductionId", "UserId", "Visible" },
                values: new object[,]
                {
                    { 1, "lampe.webp", 2, "Lampe", 1, "2", true },
                    { 2, "gaz-extracteur.webp", 2, "Extracteur de gaz", 3, "2", false },
                    { 3, "eau-extracteur.webp", 3, "Extracteur d'eau", 2, "2", true }
                });

            migrationBuilder.InsertData(
                table: "Species",
                columns: new[] { "IdSpecies", "Avatar", "Name", "RegimId", "Size", "TypeId", "UserId", "Visible", "Weight" },
                values: new object[,]
                {
                    { 1, "lion.jpg", "Lion", 2, 180, 1, "2", true, 150000.0 },
                    { 2, "boa.jpg", "Boa", 2, 225, 4, "2", true, 13000.0 },
                    { 3, "hetre.webp", "Hetre", 1, 600, 16, "2", true, 830000.0 },
                    { 4, "chene.webp", "Chene", 1, 2500, 16, "2", true, 2100000.0 },
                    { 5, "mouche.jpg", "Mouche", 4, 1, 8, "2", true, 1.0 },
                    { 6, "araigne.jpg", "Araignée", 2, 4, 7, "2", true, 95.0 },
                    { 7, "hirondelle.jpg", "Hirondelle", 2, 18, 2, "2", true, 45.0 },
                    { 8, "lievrejpeg.jpeg", "Lievre", 5, 50, 1, "2", true, 4000.0 },
                    { 9, "paturin.jpg", "Pâturin des prés", 1, 23, 16, "2", true, 1.0 },
                    { 10, "souris.jpg", "Souris", 3, 9, 1, "2", true, 25.0 }
                });

            migrationBuilder.InsertData(
                table: "BiodomeHistory",
                columns: new[] { "Id", "BiodomeId", "Delta", "SpeciesId", "UpdateAt" },
                values: new object[,]
                {
                    { 1, 4, 5, 10, new DateTime(2026, 4, 10, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, 4, -3, 10, new DateTime(2026, 4, 20, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, 4, 1, 10, new DateTime(2026, 5, 5, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, 4, 15, 9, new DateTime(2026, 4, 10, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, 4, -8, 9, new DateTime(2026, 4, 22, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, 4, 3, 9, new DateTime(2026, 5, 3, 11, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, 5, 4, 1, new DateTime(2026, 4, 10, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, 5, -3, 1, new DateTime(2026, 4, 18, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, 5, 1, 1, new DateTime(2026, 5, 2, 11, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, 5, 90, 8, new DateTime(2026, 4, 10, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, 5, -25, 8, new DateTime(2026, 4, 25, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, 5, 10, 8, new DateTime(2026, 5, 5, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, 5, 8, 3, new DateTime(2026, 4, 10, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 14, 5, -4, 3, new DateTime(2026, 4, 28, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, 5, 1, 3, new DateTime(2026, 5, 7, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 16, 1, 4, 1, new DateTime(2026, 4, 10, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 17, 1, -3, 1, new DateTime(2026, 4, 18, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { 18, 1, 1, 1, new DateTime(2026, 5, 2, 11, 0, 0, 0, DateTimeKind.Utc) },
                    { 19, 1, 90, 8, new DateTime(2026, 4, 10, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 20, 1, -25, 8, new DateTime(2026, 4, 25, 14, 0, 0, 0, DateTimeKind.Utc) },
                    { 21, 1, 10, 8, new DateTime(2026, 5, 5, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { 22, 1, 8, 3, new DateTime(2026, 4, 10, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 23, 1, -4, 3, new DateTime(2026, 4, 28, 16, 0, 0, 0, DateTimeKind.Utc) },
                    { 24, 1, 1, 3, new DateTime(2026, 5, 7, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "BiodomeSpecies",
                columns: new[] { "BiodomeId", "SpeciesId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 2 },
                    { 1, 3, 5 },
                    { 1, 8, 75 },
                    { 4, 9, 10 },
                    { 4, 10, 3 },
                    { 5, 1, 2 },
                    { 5, 3, 5 },
                    { 5, 8, 75 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

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
                name: "IX_AspNetUsers_ResearchOrganismId",
                table: "AspNetUsers",
                column: "ResearchOrganismId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BiodomeEquipment_EquipmentId",
                table: "BiodomeEquipment",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_BiodomeHistory_BiodomeId",
                table: "BiodomeHistory",
                column: "BiodomeId");

            migrationBuilder.CreateIndex(
                name: "IX_BiodomeHistory_SpeciesId",
                table: "BiodomeHistory",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Biodomes_userId",
                table: "Biodomes",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_BiodomeSpecies_SpeciesId",
                table: "BiodomeSpecies",
                column: "SpeciesId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_ConsumptionId",
                table: "Equipment",
                column: "ConsumptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_ProductionId",
                table: "Equipment",
                column: "ProductionId");

            migrationBuilder.CreateIndex(
                name: "IX_Equipment_UserId",
                table: "Equipment",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_RegimId",
                table: "Species",
                column: "RegimId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_TypeId",
                table: "Species",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Species_UserId",
                table: "Species",
                column: "UserId");
        }

        /// <inheritdoc />
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
                name: "BiodomeEquipment");

            migrationBuilder.DropTable(
                name: "BiodomeHistory");

            migrationBuilder.DropTable(
                name: "BiodomeSpecies");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Biodomes");

            migrationBuilder.DropTable(
                name: "Species");

            migrationBuilder.DropTable(
                name: "Consumptions");

            migrationBuilder.DropTable(
                name: "Productions");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SpeciesRegimes");

            migrationBuilder.DropTable(
                name: "SpeciesTypes");

            migrationBuilder.DropTable(
                name: "ResearchOrganisms");
        }
    }
}
