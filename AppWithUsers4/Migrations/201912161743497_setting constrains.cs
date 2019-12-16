namespace AppWithUsers4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class settingconstrains : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "NameOfUser", c => c.String(maxLength: 50));
            AlterColumn("dbo.AspNetUsers", "Surname", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Surname", c => c.String());
            AlterColumn("dbo.AspNetUsers", "NameOfUser", c => c.String());
        }
    }
}
