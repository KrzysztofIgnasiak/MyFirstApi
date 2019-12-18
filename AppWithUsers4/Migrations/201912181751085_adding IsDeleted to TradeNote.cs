namespace AppWithUsers4.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingIsDeletedtoTradeNote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TradeNotes", "IsDeleted", c => c.Boolean(nullable: false,defaultValue:false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TradeNotes", "IsDeleted");
        }
    }
}
