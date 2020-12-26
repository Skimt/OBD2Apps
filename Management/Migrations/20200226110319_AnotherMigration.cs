using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Management.Migrations
{
    public partial class AnotherMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarBrand",
                columns: table => new
                {
                    CarBrandId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarBrand", x => x.CarBrandId);
                });

            migrationBuilder.CreateTable(
                name: "CarModel",
                columns: table => new
                {
                    CarModelId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarModel", x => x.CarModelId);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    PersonId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GivenName = table.Column<string>(unicode: false, maxLength: 16, nullable: true),
                    SurName = table.Column<string>(unicode: false, maxLength: 16, nullable: true),
                    IdentityId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.PersonId);
                });

            migrationBuilder.CreateTable(
                name: "Car",
                columns: table => new
                {
                    CarId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Volume = table.Column<double>(nullable: true),
                    Year = table.Column<DateTime>(type: "date", nullable: true),
                    Fuel = table.Column<byte>(nullable: true),
                    CarBrandId = table.Column<int>(nullable: true),
                    CarModelId = table.Column<int>(nullable: true),
                    PersonId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Car", x => x.CarId);
                    table.ForeignKey(
                        name: "FK__Car__BrandId__797309D9",
                        column: x => x.CarBrandId,
                        principalTable: "CarBrand",
                        principalColumn: "CarBrandId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Car__ModelId__7B5B524B",
                        column: x => x.CarModelId,
                        principalTable: "CarModel",
                        principalColumn: "CarModelId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK__Car__PersonId__3C69FB99",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "PersonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    SessionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateStart = table.Column<DateTime>(type: "datetime", nullable: true),
                    DateStop = table.Column<DateTime>(type: "datetime", nullable: true),
                    CarId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK__Session_CarId_60A75C0F",
                        column: x => x.CarId,
                        principalTable: "Car",
                        principalColumn: "CarId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LogEntry",
                columns: table => new
                {
                    LogEntryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    SessionId = table.Column<int>(nullable: true),
                    PID = table.Column<string>(unicode: false, maxLength: 2, nullable: true),
                    Value = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntry", x => x.LogEntryId);
                    table.ForeignKey(
                        name: "FK_LogEntry_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Session",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Car_CarBrandId",
                table: "Car",
                column: "CarBrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Car_CarModelId",
                table: "Car",
                column: "CarModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Car_PersonId",
                table: "Car",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_LogEntry_SessionId",
                table: "LogEntry",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Session_CarId",
                table: "Session",
                column: "CarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogEntry");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "Car");

            migrationBuilder.DropTable(
                name: "CarBrand");

            migrationBuilder.DropTable(
                name: "CarModel");

            migrationBuilder.DropTable(
                name: "Person");
        }
    }
}
