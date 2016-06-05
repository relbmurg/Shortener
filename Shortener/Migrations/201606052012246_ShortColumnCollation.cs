namespace Shortener.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ShortColumnCollation : DbMigration
    {
        public override void Up()
        {
            Sql("ALTER TABLE [dbo].[ShortUrls] ALTER COLUMN [Short] NVARCHAR(MAX) COLLATE SQL_Latin1_General_CP1_CS_AS NULL");
        }
        
        public override void Down()
        {
            Sql("ALTER TABLE [dbo].[ShortUrls] ALTER COLUMN [Short] NVARCHAR(MAX) NULL");
        }
    }
}
