namespace MovieDatabase.DataAccess.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Directors",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    FirstName = c.String(),
                    LastName = c.String(),
                    DateOfBirth = c.DateTime(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "dbo.Movies",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Title = c.String(),
                    Duration = c.Time(nullable: false, precision: 7),
                    Director_Id = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Directors", t => t.Director_Id)
                .Index(t => t.Director_Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Movies", "Director_Id", "dbo.Directors");
            DropIndex("dbo.Movies", new[] { "Director_Id" });
            DropTable("dbo.Movies");
            DropTable("dbo.Directors");
        }
    }
}
