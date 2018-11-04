namespace MovieDatabase.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDirector : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Directors", "Name", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Directors", "FirstName");
            DropColumn("dbo.Directors", "LastName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Directors", "LastName", c => c.String());
            AddColumn("dbo.Directors", "FirstName", c => c.String(nullable: false, maxLength: 50));
            DropColumn("dbo.Directors", "Name");
        }
    }
}
