using System;

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DndPlatform.Persistence.Migrations;

[DbContext(typeof(StorageContext))]
[Migration("20260919220000_SeedSenderTypes")]
public sealed class SeedSenderTypes : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        var timestamp = new DateTime(2026, 9, 19, 0, 0, 0, DateTimeKind.Utc);

        migrationBuilder.InsertData(
            table: "SenderTypes",
            columns: new[] { "Id", "Name", "Code", "DateCreate", "DateUpdate" },
            values: new object[,]
            {
                { new Guid("f0a904e2-5035-4b33-9803-7712b8065150"), "System", "System", timestamp, timestamp },
                { new Guid("089e3c58-0a06-4b45-b36c-a166ccd06376"), "User", "User", timestamp, timestamp },
                { new Guid("7c6bb3e5-51be-4c91-b859-6c3cb65ed37f"), "Assistant", "Assistant", timestamp, timestamp },
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DeleteData(
            table: "SenderTypes",
            keyColumn: "Id",
            keyValues: new object[]
            {
                new Guid("f0a904e2-5035-4b33-9803-7712b8065150"),
                new Guid("089e3c58-0a06-4b45-b36c-a166ccd06376"),
                new Guid("7c6bb3e5-51be-4c91-b859-6c3cb65ed37f"),
            });
    }
}
