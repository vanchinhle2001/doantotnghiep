namespace DoAn_LapTrinhWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class brand_typeToBrand : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Brand", "brand_type", c => c.String(nullable: false, maxLength: 10));
            AddColumn("dbo.Genre", "genre_type", c => c.String(nullable: false, maxLength: 10));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Genre", "genre_type");
            DropColumn("dbo.Brand", "brand_type");
        }
    }
}
