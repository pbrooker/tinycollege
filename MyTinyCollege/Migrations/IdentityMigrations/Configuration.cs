namespace MyTinyCollege.Migrations.IdentityMigrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    //pbrooker
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    internal sealed class Configuration : DbMigrationsConfiguration<MyTinyCollege.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\IdentityMigrations";
            ContextKey = "MyTinyCollege.Models.ApplicationDbContext";
        }

        protected override void Seed(MyTinyCollege.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            // pbrooker seeding identity with roles and admin user

            //1. Add admin role
            if (!(context.Roles.Any(r => r.Name == "admin")))
            {
                //role does not exist - create it
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var roleToInstert = new IdentityRole { Name = "admin" };
                roleManager.Create(roleToInstert);
            }


            //2. Add student roll
            if (!(context.Roles.Any(r => r.Name == "student")))
            {
                //role does not exist - create it
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var roleToInstert = new IdentityRole { Name = "student" };
                roleManager.Create(roleToInstert);
            }


            //3. Add instructor role
            if (!(context.Roles.Any(r => r.Name == "instructor")))
            {
                //role does not exist - create it
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var roleToInstert = new IdentityRole { Name = "instructor" };
                roleManager.Create(roleToInstert);
            }

            //4. Add admin user and assing admin role
            if (!(context.Users.Any(r => r.UserName == "admin@tinycollege.com")))
            {
                //admin user does not exist - create it
                var userStore = new UserStore<Models.ApplicationUser>(context);
                var userManager = new UserManager<Models.ApplicationUser>(userStore);
                var userToInsert = new Models.ApplicationUser
                {
                    UserName = "admin@tinycollege.com",
                    Email = "admin@tineycollege.com",
                    EmailConfirmed = true,
                };
                //userManager.Create(userToInsert, "");

                //assign admin user to admin role
                userManager.AddToRole(userToInsert.Id, "admin");

            }
        }
    }
}
