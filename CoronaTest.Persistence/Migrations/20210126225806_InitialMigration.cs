using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoronaTest.Persistence.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Campaigns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    From = table.Column<DateTime>(nullable: false),
                    To = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campaigns", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Firstname = table.Column<string>(maxLength: 20, nullable: false),
                    Lastname = table.Column<string>(nullable: false),
                    SocialSecurityNumber = table.Column<string>(maxLength: 10, nullable: false),
                    Mobilephone = table.Column<string>(maxLength: 16, nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    Gender = table.Column<string>(nullable: true),
                    Birthdate = table.Column<DateTime>(nullable: false),
                    Postalcode = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    HouseNumber = table.Column<string>(nullable: true),
                    Stair = table.Column<string>(nullable: true),
                    Door = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestCenters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Name = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    Postalcode = table.Column<string>(nullable: true),
                    Street = table.Column<string>(nullable: true),
                    SlotCapacity = table.Column<int>(nullable: false),
                    CampaignId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCenters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestCenters_Campaigns_CampaignId",
                        column: x => x.CampaignId,
                        principalTable: "Campaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "VerificationTokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    Token = table.Column<int>(nullable: false),
                    Identifier = table.Column<Guid>(nullable: false),
                    IssuedAt = table.Column<DateTime>(nullable: false),
                    IsInvalidated = table.Column<bool>(nullable: false),
                    ParticipantId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerificationTokens_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Examinations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RowVersion = table.Column<byte[]>(rowVersion: true, nullable: true),
                    ExaminationAtId = table.Column<int>(nullable: true),
                    Identifier = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Examinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Examinations_TestCenters_ExaminationAtId",
                        column: x => x.ExaminationAtId,
                        principalTable: "TestCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Examinations_ExaminationAtId",
                table: "Examinations",
                column: "ExaminationAtId");

            migrationBuilder.CreateIndex(
                name: "IX_TestCenters_CampaignId",
                table: "TestCenters",
                column: "CampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_VerificationTokens_ParticipantId",
                table: "VerificationTokens",
                column: "ParticipantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Examinations");

            migrationBuilder.DropTable(
                name: "VerificationTokens");

            migrationBuilder.DropTable(
                name: "TestCenters");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "Campaigns");
        }
    }
}
