﻿using GroupSpace23.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GroupSpace23.Models;
using System.Reflection;
using System.Data;
using System.Security.Cryptography.Pkcs;
using GroupSpace23.Migrations;
using System.Reflection.Emit;

namespace GroupSpace23.Data;

public class MyDbContext : IdentityDbContext<GroupSpace23User>
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }
 


    public static async Task DataInitializer(MyDbContext context, UserManager<GroupSpace23User> userManager)
    {

        if (!context.Languages.Any())
        {
            context.AddRange(
                new Language { Id = "- ", Name = "-", IsSystemLanguage = false, IsAvailable = DateTime.MaxValue },
                new Language { Id = "en", Name = "English", IsSystemLanguage = true },
                new Language { Id = "nl", Name = "Nederlands", IsSystemLanguage = true },
                new Language { Id = "fr", Name = "français", IsSystemLanguage = true },
                new Language { Id = "de", Name = "Deutsch", IsSystemLanguage = true }
                );
            context.SaveChanges();
        }

        Models.Language.GetLanguages(context);
        if (!context.Users.Any())
        {
            GroupSpace23User dummyuser = new GroupSpace23User
            {
                Id = "Dummy",
                Email = "dummy@dummy.xx",
                UserName = "Dummy",
                FirstName = "Dummy",
                LastName = "Dummy",
                PasswordHash = "Dummy",
                LockoutEnabled = true,
                LockoutEnd = DateTime.MaxValue
            };
            context.Users.Add(dummyuser);
            context.SaveChanges();


            GroupSpace23User adminUser = new GroupSpace23User
            {
                Id = "Admin",
                Email = "admin@dummy.xx",
                UserName = "Admin",
                FirstName = "Administrator",
                LastName = "GroupSpace"
            };



            var result = await userManager.CreateAsync(adminUser, "Abc!12345");

        }

        GroupSpace23User dummy = context.Users.First(u => u.UserName == "Dummy");
        GroupSpace23User admin = context.Users.First(u => u.UserName == "Admin");
        AddParameters(context, admin);


        Globals.DummyUser = dummy;

        if (!context.Roles.Any())
        {
            context.Roles.AddRange(
                new IdentityRole { Name = "SystemAdministrator", Id = "SystemAdministrator", NormalizedName = "SYSTEMADMINISTRATOR" },
                new IdentityRole { Name = "User", Id = "User", NormalizedName = "USER" },
                new IdentityRole { Name = "UserAdministrator", Id = "UserAdministrator", NormalizedName = "USERADMINISTRATOR" }
            );

            context.UserRoles.Add(new IdentityUserRole<string> { RoleId = "SystemAdministrator", UserId = admin.Id });
            context.UserRoles.Add(new IdentityUserRole<string> { RoleId = "UserAdministrator", UserId = admin.Id });

            context.SaveChanges();
        }

        if (!context.Leverancier.Any())
        {
            context.Leverancier.Add(new Leverancier { Description = "Leveranciers voor kleren", Name = "Alex Turkey" ,email = "AlexFromTurkey@gmail.com"});
            context.SaveChanges();
        }



        if (!context.Projecten.Any())
        {
            context.Projecten.Add(new Project { Description = "Dummy", Name = "Dummy", Ended = DateTime.Now });
            context.SaveChanges();
        }
        Project dummyGroup = context.Projecten.FirstOrDefault(g => g.Name == "Dummy");
        List<Project> groups = context.Projecten.ToList();
        //foreach (Group group in groups)
        //{

        //    group.StartedById = dummy.Id;
        //    context.Update(group);


        //}
        //context.SaveChanges();


        if (!context.Messages.Any())
        {
            context.Messages.Add(new Models.Message { Title = "Dummy", Body = "", Sent = DateTime.Now, Deleted = DateTime.Now, Recipient = dummyGroup });
            context.SaveChanges();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<InventoryItem>()
        //    .HasOne(item => item.Owner)
        //    .WithMany()
        //    .HasForeignKey(item => item.OwnerId)
        //    .OnDelete(DeleteBehavior.NoAction); // or .OnDelete(DeleteBehavior.Restrict) if you prefer

        //modelBuilder.Entity<InventoryItem>()
        //    .HasOne(item => item.Project)
        //    .WithMany()
        //    .HasForeignKey(item => item.ProjectId)
        //    .OnDelete(DeleteBehavior.NoAction); // or .OnDelete(DeleteBehavior.Restrict) if you prefer

    }

    static void AddParameters(MyDbContext context, GroupSpace23User user)
    {
        if (!context.Parameters.Any())
        {
            context.Parameters.AddRange(
                new Parameter { Name = "Version", Value = "0.1.0", Description = "Huidige versie van de parameterlijst", Destination = "System", UserId = user.Id },
                new Parameter { Name = "Mail.Server", Value = "newmail.server.com", Description = "Naam van de gebruikte pop-server", Destination = "Mail", UserId = user.Id },
                new Parameter { Name = "Mail.Port", Value = "25", Description = "Poort van de smtp-server", Destination = "Mail", UserId = user.Id },
                new Parameter { Name = "Mail.Account", Value = "SmtpServer", Description = "Acount-naam van de smtp-server", Destination = "Mail", UserId = user.Id },
                new Parameter { Name = "Mail.Password", Value = "xxxyyy!2315", Description = "Wachtwoord van de smtp-server", Destination = "Mail", UserId = user.Id },
                new Parameter { Name = "Mail.Security", Value = "true", Description = "Is SSL or TLS encryption used (true or false)", Destination = "Mail", UserId = user.Id },
                new Parameter { Name = "Mail.SenderEmail", Value = "administrator.groupspace.be", Description = "E-mail van de smtp-verzender", Destination = "Mail", UserId = user.Id },
                new Parameter { Name = "Mail.SenderName", Value = "Administrator", Description = "Naam van de smtp-verzender", Destination = "Mail", UserId = user.Id }
            );
            context.SaveChanges();
        }
        Globals.Parameters = new Dictionary<string, Parameter>();
        foreach (Parameter parameter in context.Parameters)
        {
            Globals.Parameters[parameter.Name] = parameter;
        }
        Globals.ConfigureMail();
    }


    public DbSet<GroupSpace23.Models.Project> Projecten{ get; set; }


    public DbSet<GroupSpace23.Models.Parameter> Parameters { get; set; }


    public DbSet<GroupSpace23.Models.Message> Messages { get; set; }


    public DbSet<GroupSpace23.Models.GroupMember> GroupMembers { get; set; }


    public DbSet<GroupSpace23User> Users { get; set; }


    public DbSet<GroupSpace23.Models.Language> Languages { get; set; } = default!;


    public DbSet<GroupSpace23.Models.Leverancier> Leverancier { get; set; } = default!;


    public DbSet<GroupSpace23.Models.InventoryItem> InventoryItem { get; set; } = default!;


}


