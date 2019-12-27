namespace AppWithUsers4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class becauseitdoesnotwork : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ContactPersons", name: "CompanyId_Id_Id", newName: "CompanyId_Id");
            RenameIndex(table: "dbo.ContactPersons", name: "IX_CompanyId_Id_Id", newName: "IX_CompanyId_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ContactPersons", name: "IX_CompanyId_Id", newName: "IX_CompanyId_Id_Id");
            RenameColumn(table: "dbo.ContactPersons", name: "CompanyId_Id", newName: "CompanyId_Id_Id");
        }
    }
}
