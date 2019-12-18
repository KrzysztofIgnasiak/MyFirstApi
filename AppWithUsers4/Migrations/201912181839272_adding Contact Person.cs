namespace AppWithUsers4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingContactPerson : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ContactPersons",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        Surname = c.String(nullable: false, maxLength: 50),
                        Phone = c.Int(),
                        Mail = c.String(nullable: false, maxLength: 50),
                        Position = c.String(maxLength: 100),
                        isDeleted = c.Boolean(nullable: false,defaultValue:false),
                        UserId_Id = c.String(maxLength: 128),
                        CompanyId_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId_Id)
                .ForeignKey("dbo.Companies", t => t.CompanyId_Id)
                .Index(t => t.UserId_Id)
                .Index(t => t.CompanyId_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ContactPersons", "CompanyId_Id", "dbo.Companies");
            DropForeignKey("dbo.ContactPersons", "UserId_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ContactPersons", new[] { "CompanyId_Id" });
            DropIndex("dbo.ContactPersons", new[] { "UserId_Id" });
            DropTable("dbo.ContactPersons");
        }
    }
}
