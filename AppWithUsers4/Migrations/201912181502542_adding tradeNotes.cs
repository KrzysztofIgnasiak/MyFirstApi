namespace AppWithUsers4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingtradeNotes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TradeNotes",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        Text = c.String(nullable: false),
                        UserId_Id = c.String(maxLength: 128),
                        CompanyId_Id = c.Int(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId_Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId_Id)
                .Index(t => t.UserId_Id)
                .Index(t => t.CompanyId_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TradeNotes", "CompanyId_Id", "dbo.Companies");
            DropForeignKey("dbo.TradeNotes", "UserId_Id", "dbo.AspNetUsers");
            DropIndex("dbo.TradeNotes", new[] { "CompanyId_Id" });
            DropIndex("dbo.TradeNotes", new[] { "UserId_Id" });
            DropTable("dbo.TradeNotes");
        }
    }
}
