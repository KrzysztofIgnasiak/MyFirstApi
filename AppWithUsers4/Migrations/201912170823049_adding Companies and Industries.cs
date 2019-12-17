namespace AppWithUsers4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingCompaniesandIndustries : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Nip = c.Int(nullable: false),
                        Address = c.String(maxLength: 150),
                        City = c.String(maxLength: 150),
                        IsDeleted = c.Boolean(nullable: false,defaultValue:false),
                        IndustryType_Id = c.Int(),
                        userID_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Industries", t => t.IndustryType_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.userID_Id)
                .Index(t => t.IndustryType_Id)
                .Index(t => t.userID_Id);
            
            CreateTable(
                "dbo.Industries",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Companies", "userID_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Companies", "IndustryType_Id", "dbo.Industries");
            DropIndex("dbo.Companies", new[] { "userID_Id" });
            DropIndex("dbo.Companies", new[] { "IndustryType_Id" });
            DropTable("dbo.Industries");
            DropTable("dbo.Companies");
        }
    }
}
