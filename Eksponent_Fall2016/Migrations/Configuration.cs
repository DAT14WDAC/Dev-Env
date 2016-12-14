namespace Eksponent_Fall2016.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Eksponent_Fall2016.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Eksponent_Fall2016.Models.ApplicationDbContext";
        }

        protected override void Seed(Eksponent_Fall2016.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            string roleAdmin = "Admin";
            string roleUser = "Employee";

            //Create Role Admin if it does not exist
            if (!roleManager.RoleExists(roleAdmin))
            {
                var roleResult = roleManager.Create(new IdentityRole(roleAdmin));
                
            }
            //Create Role Employee if it does not exist
            if (!roleManager.RoleExists(roleUser))
            {
                var roleResult = roleManager.Create(new IdentityRole(roleUser));

            }
        }
    }
}
