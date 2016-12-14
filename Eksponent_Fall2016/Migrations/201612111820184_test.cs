namespace Eksponent_Fall2016.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Companies", "CompanyName", c => c.String());
            AddColumn("dbo.Companies", "ApplicationUserId", c => c.String());
            AddColumn("dbo.Employees", "ApplicationUserId", c => c.String());
            AddColumn("dbo.Skills", "Skilldescription", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Skills", "Skilldescription");
            DropColumn("dbo.Employees", "ApplicationUserId");
            DropColumn("dbo.Companies", "ApplicationUserId");
            DropColumn("dbo.Companies", "CompanyName");
        }
    }
}
