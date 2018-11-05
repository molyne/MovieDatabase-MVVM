namespace MovieDatabase.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDirector : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Actors", "Name", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Actors", "FirstName");
            DropColumn("dbo.Actors", "LastName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Actors", "LastName", c => c.String());
            AddColumn("dbo.Actors", "FirstName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Actors", "Name");
        }
    }
}
