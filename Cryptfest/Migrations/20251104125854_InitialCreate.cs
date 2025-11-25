using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cryptfest.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CryptoAssetMarketData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrPrice = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    PercentChange1h = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    PercentChange24h = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    PercentChange7d = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    PercentChange30d = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    PercentChange60d = table.Column<decimal>(type: "decimal(18,8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoAssetMarketData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserLogInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WalletStatistics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalAssets = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    TotalDeposit = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    Apy = table.Column<decimal>(type: "decimal(18,8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletStatistics", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CryptoAsset",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MarketDataId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoAsset", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CryptoAsset_CryptoAssetMarketData_MarketDataId",
                        column: x => x.MarketDataId,
                        principalTable: "CryptoAssetMarketData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserLogInfoId = table.Column<int>(type: "int", nullable: false),
                    ClientRequestId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_ClientRequests_ClientRequestId",
                        column: x => x.ClientRequestId,
                        principalTable: "ClientRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_UserLogInfo_UserLogInfoId",
                        column: x => x.UserLogInfoId,
                        principalTable: "UserLogInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatisticId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wallets_WalletStatistics_StatisticId",
                        column: x => x.StatisticId,
                        principalTable: "WalletStatistics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CryptoBalances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    Usdt = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    PurchasePrice = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    WalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CryptoBalances_CryptoAsset_AssetId",
                        column: x => x.AssetId,
                        principalTable: "CryptoAsset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CryptoBalances_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CryptoExchanges",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,8)", nullable: false),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    FromAssetId = table.Column<int>(type: "int", nullable: true),
                    ToAssetId = table.Column<int>(type: "int", nullable: true),
                    WalletId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoExchanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CryptoExchanges_CryptoAsset_FromAssetId",
                        column: x => x.FromAssetId,
                        principalTable: "CryptoAsset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CryptoExchanges_CryptoAsset_ToAssetId",
                        column: x => x.ToAssetId,
                        principalTable: "CryptoAsset",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CryptoExchanges_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CryptoAsset_MarketDataId",
                table: "CryptoAsset",
                column: "MarketDataId");

            migrationBuilder.CreateIndex(
                name: "IX_CryptoBalances_AssetId",
                table: "CryptoBalances",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_CryptoBalances_WalletId",
                table: "CryptoBalances",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_CryptoExchanges_FromAssetId",
                table: "CryptoExchanges",
                column: "FromAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_CryptoExchanges_ToAssetId",
                table: "CryptoExchanges",
                column: "ToAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_CryptoExchanges_WalletId",
                table: "CryptoExchanges",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClientRequestId",
                table: "Users",
                column: "ClientRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserLogInfoId",
                table: "Users",
                column: "UserLogInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_StatisticId",
                table: "Wallets",
                column: "StatisticId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CryptoBalances");

            migrationBuilder.DropTable(
                name: "CryptoExchanges");

            migrationBuilder.DropTable(
                name: "CryptoAsset");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "CryptoAssetMarketData");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WalletStatistics");

            migrationBuilder.DropTable(
                name: "ClientRequests");

            migrationBuilder.DropTable(
                name: "UserLogInfo");
        }
    }
}
