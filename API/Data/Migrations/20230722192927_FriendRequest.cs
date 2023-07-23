using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace API.Data.Migrations
{
    /// <inheritdoc />
    public partial class FriendRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUserAppUser",
                columns: table => new
                {
                    FriendOfUsersId = table.Column<int>(type: "integer", nullable: false),
                    FriendsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserAppUser", x => new { x.FriendOfUsersId, x.FriendsId });
                    table.ForeignKey(
                        name: "FK_AppUserAppUser_Users_FriendOfUsersId",
                        column: x => x.FriendOfUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserAppUser_Users_FriendsId",
                        column: x => x.FriendsId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RequestFromId = table.Column<int>(type: "integer", nullable: false),
                    RequestToId = table.Column<int>(type: "integer", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendRequests_Users_RequestFromId",
                        column: x => x.RequestFromId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendRequests_Users_RequestToId",
                        column: x => x.RequestToId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUserAppUser_FriendsId",
                table: "AppUserAppUser",
                column: "FriendsId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_RequestFromId",
                table: "FriendRequests",
                column: "RequestFromId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_RequestToId",
                table: "FriendRequests",
                column: "RequestToId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUserAppUser");

            migrationBuilder.DropTable(
                name: "FriendRequests");
        }
    }
}
