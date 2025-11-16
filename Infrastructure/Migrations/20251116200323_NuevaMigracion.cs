using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NuevaMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventSector",
                columns: table => new
                {
                    EventSectorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    IsControlled = table.Column<bool>(type: "bit", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    SoldCount = table.Column<int>(type: "int", nullable: false),
                    ReservedCount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSector", x => x.EventSectorId);
                });

            migrationBuilder.CreateTable(
                name: "TicketStatus",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(25)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketStatus", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "EventSeat",
                columns: table => new
                {
                    EventSeatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventSectorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SeatId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    ReservedByUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSeat", x => x.EventSeatId);
                    table.ForeignKey(
                        name: "FK_EventSeat_EventSector_EventSectorId",
                        column: x => x.EventSectorId,
                        principalTable: "EventSector",
                        principalColumn: "EventSectorId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventSeat_TicketStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "TicketStatus",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.TicketId);
                    table.ForeignKey(
                        name: "FK_Ticket_TicketStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "TicketStatus",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketSeat",
                columns: table => new
                {
                    TicketSeatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventSeatId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventSeatId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketSeat", x => x.TicketSeatId);
                    table.ForeignKey(
                        name: "FK_TicketSeat_EventSeat_EventSeatId",
                        column: x => x.EventSeatId,
                        principalTable: "EventSeat",
                        principalColumn: "EventSeatId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketSeat_EventSeat_EventSeatId1",
                        column: x => x.EventSeatId1,
                        principalTable: "EventSeat",
                        principalColumn: "EventSeatId");
                    table.ForeignKey(
                        name: "FK_TicketSeat_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Ticket",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TicketSectors",
                columns: table => new
                {
                    TicketSectorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventSectorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    EventSectorId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketSectors", x => x.TicketSectorId);
                    table.ForeignKey(
                        name: "FK_TicketSectors_EventSector_EventSectorId",
                        column: x => x.EventSectorId,
                        principalTable: "EventSector",
                        principalColumn: "EventSectorId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketSectors_EventSector_EventSectorId1",
                        column: x => x.EventSectorId1,
                        principalTable: "EventSector",
                        principalColumn: "EventSectorId");
                    table.ForeignKey(
                        name: "FK_TicketSectors_Ticket_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Ticket",
                        principalColumn: "TicketId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "TicketStatus",
                columns: new[] { "StatusID", "Name" },
                values: new object[,]
                {
                    { 1, "Available" },
                    { 2, "Reserved" },
                    { 3, "Sold" },
                    { 4, "Expired" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventSeat_EventSectorId",
                table: "EventSeat",
                column: "EventSectorId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSeat_StatusId",
                table: "EventSeat",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_StatusId",
                table: "Ticket",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSeat_EventSeatId",
                table: "TicketSeat",
                column: "EventSeatId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSeat_EventSeatId1",
                table: "TicketSeat",
                column: "EventSeatId1");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSeat_TicketId",
                table: "TicketSeat",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSectors_EventSectorId",
                table: "TicketSectors",
                column: "EventSectorId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSectors_EventSectorId1",
                table: "TicketSectors",
                column: "EventSectorId1");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSectors_TicketId",
                table: "TicketSectors",
                column: "TicketId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TicketSeat");

            migrationBuilder.DropTable(
                name: "TicketSectors");

            migrationBuilder.DropTable(
                name: "EventSeat");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "EventSector");

            migrationBuilder.DropTable(
                name: "TicketStatus");
        }
    }
}
