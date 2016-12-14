namespace Eksponent_Fall2016.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alot : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Companies",
                c => new
                    {
                        CompanyId = c.Int(nullable: false, identity: true),
                        CompanyDescription = c.String(),
                        CompanyLogo = c.String(),
                    })
                .PrimaryKey(t => t.CompanyId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        Firstname = c.String(),
                        Lastname = c.String(),
                        Profileimage = c.String(),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: true)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.EmployeeSkills",
                c => new
                    {
                        EmployeeSkillId = c.Int(nullable: false, identity: true),
                        Level = c.Int(nullable: false),
                        SkillId = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeSkillId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Skills", t => t.SkillId, cascadeDelete: true)
                .Index(t => t.SkillId)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        SkillId = c.Int(nullable: false, identity: true),
                        Skillname = c.String(),
                        CompanyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SkillId)
                .ForeignKey("dbo.Companies", t => t.CompanyId, cascadeDelete: false)
                .Index(t => t.CompanyId);
            
            CreateTable(
                "dbo.SkillFocus",
                c => new
                    {
                        SkillFocusId = c.Int(nullable: false, identity: true),
                        EmployeeId = c.Int(nullable: false),
                        SkillId = c.Int(nullable: false),
                        Startdate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SkillFocusId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .ForeignKey("dbo.Skills", t => t.SkillId, cascadeDelete: true)
                .Index(t => t.EmployeeId)
                .Index(t => t.SkillId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SkillFocus", "SkillId", "dbo.Skills");
            DropForeignKey("dbo.SkillFocus", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.EmployeeSkills", "SkillId", "dbo.Skills");
            DropForeignKey("dbo.Skills", "CompanyId", "dbo.Companies");
            DropForeignKey("dbo.EmployeeSkills", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "CompanyId", "dbo.Companies");
            DropIndex("dbo.SkillFocus", new[] { "SkillId" });
            DropIndex("dbo.SkillFocus", new[] { "EmployeeId" });
            DropIndex("dbo.Skills", new[] { "CompanyId" });
            DropIndex("dbo.EmployeeSkills", new[] { "EmployeeId" });
            DropIndex("dbo.EmployeeSkills", new[] { "SkillId" });
            DropIndex("dbo.Employees", new[] { "CompanyId" });
            DropTable("dbo.SkillFocus");
            DropTable("dbo.Skills");
            DropTable("dbo.EmployeeSkills");
            DropTable("dbo.Employees");
            DropTable("dbo.Companies");
        }
    }
}
