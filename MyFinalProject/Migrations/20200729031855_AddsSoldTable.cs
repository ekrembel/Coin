using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyFinalProject.Migrations
{
    public partial class AddsSoldTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Share");

            migrationBuilder.CreateTable(
                name: "Bought",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionType = table.Column<string>(nullable: true),
                    DateAndTime = table.Column<DateTime>(nullable: false),
                    Symbol = table.Column<string>(nullable: true),
                    companyName = table.Column<string>(nullable: true),
                    NumOfShare = table.Column<int>(nullable: false),
                    latestPrice = table.Column<double>(nullable: false),
                    IsOwned = table.Column<bool>(nullable: false),
                    AspNetUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bought", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bought_ApplicationUser_AspNetUserId",
                        column: x => x.AspNetUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sold",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateAndTime = table.Column<DateTime>(nullable: false),
                    Symbol = table.Column<string>(nullable: true),
                    companyName = table.Column<string>(nullable: true),
                    NumOfShare = table.Column<int>(nullable: false),
                    latestPrice = table.Column<double>(nullable: false),
                    Cost = table.Column<double>(nullable: false),
                    Profit = table.Column<double>(nullable: false),
                    AspNetUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sold", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sold_ApplicationUser_AspNetUserId",
                        column: x => x.AspNetUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bought_AspNetUserId",
                table: "Bought",
                column: "AspNetUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sold_AspNetUserId",
                table: "Sold",
                column: "AspNetUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bought");

            migrationBuilder.DropTable(
                name: "Sold");

            migrationBuilder.CreateTable(
                name: "Share",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AspNetUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    DateAndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsOwned = table.Column<bool>(type: "bit", nullable: false),
                    NumOfShare = table.Column<int>(type: "int", nullable: false),
                    Symbol = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    companyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    latestPrice = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Share", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Share_ApplicationUser_AspNetUserId",
                        column: x => x.AspNetUserId,
                        principalTable: "ApplicationUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Share_AspNetUserId",
                table: "Share",
                column: "AspNetUserId");
        }
    }
}
