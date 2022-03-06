using Microsoft.EntityFrameworkCore.Migrations;

namespace ReactShope.Migrations
{
    public partial class BasketEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Baskets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuyerId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Baskets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<long>(type: "bigint", nullable: false),
                    PictureUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuantityInStock = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BasketItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    BasketId = table.Column<int>(type: "int", nullable: false),
                    Temp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasketItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasketItem_Baskets_BasketId",
                        column: x => x.BasketId,
                        principalTable: "Baskets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BasketItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Brand", "Description", "Name", "PictureUrl", "Price", "QuantityInStock", "Type" },
                values: new object[,]
                {
                    { 1, "HP", "HP Chromebook 14 Intel Celeron N4020-4GB S… HPHP", "HP Chromebook 14", "https://images-eu.ssl-images-amazon.com/images/I/513LujQCoXL._AC_SX184_.jpg", 300L, 18, "Laptop" },
                    { 2, "boat", "boAt Rockerz 255 Pro in-Ear Earphones with 10… boAtboAt", "boAt Rockerz 255 Pro", "https://images-eu.ssl-images-amazon.com/images/I/31PU4kWou+L._AC_SX184_.jpg", 1200L, 20, "HeadPhone" },
                    { 3, "Nicon", "Nikon Z7 Mirrorless Camera Body with 24-… NikonNikon", "Nikon Z7", "https://images-eu.ssl-images-amazon.com/images/I/41ecM6cGpxL._AC_SX184_.jpg", 1200L, 20, "HeadPhone" },
                    { 4, "LG", "LG Gram Intel Evo 11th Gen Core i7 17 inches Ultra-Light Laptop (16 GB RAM, 512 GB SSD, New Windows 11 Home Preload, Iris Xe Graphics, USC -C x 2 (with Power), 1.35 kg, 17Z90P-G.AH85A2, Black)LG Gram Intel Evo 11th Gen Core i7", "LG Gram ", "https://images-eu.ssl-images-amazon.com/images/I/416Am14drmL._AC_SX184_.jpg", 200L, 10, "Laptop" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_BasketId",
                table: "BasketItem",
                column: "BasketId");

            migrationBuilder.CreateIndex(
                name: "IX_BasketItem_ProductId",
                table: "BasketItem",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasketItem");

            migrationBuilder.DropTable(
                name: "Baskets");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
