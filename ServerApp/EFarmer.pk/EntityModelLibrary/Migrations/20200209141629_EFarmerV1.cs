using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.IO;

namespace EFarmerPkModelLibrary.Migrations
{
    public partial class EFarmerV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "CATEGORIES",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    UName = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CATEGORIES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CITIES",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    GLat = table.Column<decimal>(type: "decimal(10,8)", nullable: false),
                    GLng = table.Column<decimal>(type: "decimal(11,8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CITIES", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AGRO_ITEMS",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Uname = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    WeightScale = table.Column<string>(type: "nchar(10)", maxLength: 10, nullable: false),
                    UWeightScale = table.Column<string>(type: "nchar(10)", maxLength: 10, nullable: true),
                    CategoryId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AGRO_ITEMS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AGRO_ITEMS_CATEGORIES_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "dbo",
                        principalTable: "CATEGORIES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FName = table.Column<string>(type: "varchar(max)", nullable: false),
                    LName = table.Column<string>(type: "varchar(max)", nullable: false),
                    CCompanyCode = table.Column<string>(type: "nchar(3)", maxLength: 3, nullable: false),
                    CCountryCode = table.Column<string>(type: "nchar(3)", maxLength: 3, nullable: false),
                    CPhone = table.Column<string>(type: "nchar(7)", maxLength: 7, nullable: false),
                    Address = table.Column<string>(type: "varchar(max)", nullable: false),
                    GLat = table.Column<decimal>(type: "decimal(10,8)", nullable: true),
                    GLng = table.Column<decimal>(type: "decimal(11,8)", nullable: true),
                    BuyerFlag = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    SellerFlag = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "0"),
                    CityId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_USERS_CITIES_CityId",
                        column: x => x.CityId,
                        principalSchema: "dbo",
                        principalTable: "CITIES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ADVERTISEMENTS",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Quality = table.Column<short>(type: "smallint", nullable: false),
                    Quantity = table.Column<short>(type: "smallint", nullable: false),
                    PostedDateTime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    Picture = table.Column<string>(type: "varchar(max)", nullable: false),
                    CityId = table.Column<short>(type: "smallint", nullable: false),
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    SellerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ADVERTISEMENTS", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ADVERTISEMENTS_CITIES_CityId",
                        column: x => x.CityId,
                        principalSchema: "dbo",
                        principalTable: "CITIES",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ADVERTISEMENTS_AGRO_ITEMS_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "dbo",
                        principalTable: "AGRO_ITEMS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ADVERTISEMENTS_USERS_SellerId",
                        column: x => x.SellerId,
                        principalSchema: "dbo",
                        principalTable: "USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BUYERS_ADD_AGRO_ITEM_TO_INTEREST",
                schema: "dbo",
                columns: table => new
                {
                    ItemId = table.Column<int>(type: "int", nullable: false),
                    BuyerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BUYERS_ADD_AGRO_ITEM_TO_INTEREST", x => new { x.BuyerId, x.ItemId });
                    table.ForeignKey(
                        name: "FK_BUYERS_ADD_AGRO_ITEM_TO_INTEREST_USERS_BuyerId",
                        column: x => x.BuyerId,
                        principalSchema: "dbo",
                        principalTable: "USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BUYERS_ADD_AGRO_ITEM_TO_INTEREST_AGRO_ITEMS_ItemId",
                        column: x => x.ItemId,
                        principalSchema: "dbo",
                        principalTable: "AGRO_ITEMS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SELLERS_FAVORITES_BUYERS",
                schema: "dbo",
                columns: table => new
                {
                    SellerId = table.Column<long>(type: "bigint", nullable: false),
                    BuyerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SELLERS_FAVORITES_BUYERS", x => new { x.SellerId, x.BuyerId });
                    table.ForeignKey(
                        name: "FK_SELLERS_FAVORITES_BUYERS_USERS_BuyerId",
                        column: x => x.BuyerId,
                        principalSchema: "dbo",
                        principalTable: "USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SELLERS_FAVORITES_BUYERS_USERS_SellerId",
                        column: x => x.SellerId,
                        principalSchema: "dbo",
                        principalTable: "USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BUYER_ADDS_DIFFERENT_ADS_TO_FAV",
                schema: "dbo",
                columns: table => new
                {
                    AdId = table.Column<long>(type: "bigint", nullable: false),
                    BuyerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BUYER_ADDS_DIFFERENT_ADS_TO_FAV", x => new { x.BuyerId, x.AdId });
                    table.ForeignKey(
                        name: "FK_BUYER_ADDS_DIFFERENT_ADS_TO_FAV_ADVERTISEMENTS_AdId",
                        column: x => x.AdId,
                        principalSchema: "dbo",
                        principalTable: "ADVERTISEMENTS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BUYER_ADDS_DIFFERENT_ADS_TO_FAV_USERS_BuyerId",
                        column: x => x.BuyerId,
                        principalSchema: "dbo",
                        principalTable: "USERS",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ADVERTISEMENTS_CityId",
                schema: "dbo",
                table: "ADVERTISEMENTS",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_ADVERTISEMENTS_ItemId",
                schema: "dbo",
                table: "ADVERTISEMENTS",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ADVERTISEMENTS_SellerId",
                schema: "dbo",
                table: "ADVERTISEMENTS",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_AGRO_ITEMS_CategoryId",
                schema: "dbo",
                table: "AGRO_ITEMS",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BUYER_ADDS_DIFFERENT_ADS_TO_FAV_AdId",
                schema: "dbo",
                table: "BUYER_ADDS_DIFFERENT_ADS_TO_FAV",
                column: "AdId");

            migrationBuilder.CreateIndex(
                name: "IX_BUYERS_ADD_AGRO_ITEM_TO_INTEREST_ItemId",
                schema: "dbo",
                table: "BUYERS_ADD_AGRO_ITEM_TO_INTEREST",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CITIES_Id",
                schema: "dbo",
                table: "CITIES",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SELLERS_FAVORITES_BUYERS_BuyerId",
                schema: "dbo",
                table: "SELLERS_FAVORITES_BUYERS",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_CityId",
                schema: "dbo",
                table: "USERS",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "UNIQUE_CONTACT",
                schema: "dbo",
                table: "USERS",
                columns: new[] { "CCompanyCode", "CCountryCode", "CPhone" },
                unique: true);
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"StoredProcedures.sql");
            migrationBuilder.Sql(File.ReadAllText(file));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BUYER_ADDS_DIFFERENT_ADS_TO_FAV",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "BUYERS_ADD_AGRO_ITEM_TO_INTEREST",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SELLERS_FAVORITES_BUYERS",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "ADVERTISEMENTS",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "AGRO_ITEMS",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "USERS",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CATEGORIES",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "CITIES",
                schema: "dbo");
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"StoredProceduresV1Down.sql");
            migrationBuilder.Sql(File.ReadAllText(file));
        }
    }
}
