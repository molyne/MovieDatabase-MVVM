namespace MovieDatabase.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedPropMovieDescription : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Movies", "Description", c => c.String(maxLength: 200));
            DropColumn("dbo.Movies", "Duration");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Movies", "Duration", c => c.Time(nullable: false, precision: 7));
            DropColumn("dbo.Movies", "Description");
        }
    }
}
