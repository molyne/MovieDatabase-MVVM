namespace MovieDatabase.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDirectorToActor : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Directors", newName: "Actors");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.Actors", newName: "Directors");
        }
    }
}
