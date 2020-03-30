using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.API.Model;

namespace TaskManager.API.Data
{
    public class DataContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole,
        IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(ur => ur.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(ur => ur.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<UserProject>(userProject =>
            {
                userProject.HasKey(up => new { up.ProjectId, up.UserId });

                userProject.HasOne(up => up.User)
                   .WithMany(up => up.UserProjects)
                   .HasForeignKey(up => up.UserId);

                userProject.HasOne(up => up.Project)
                   .WithMany(up => up.UserProjects)
                   .HasForeignKey(up => up.ProjectId);
            });


            builder.Entity<Message>(message =>
           {
               message.HasKey(m => new { m.RecipientId, m.SenderId });

               message.HasOne(m => m.Recipient)
                   .WithMany(m => m.MessagesReceived)
                   .HasForeignKey(m => m.RecipientId)
                   .OnDelete(DeleteBehavior.Restrict);

               message.HasOne(m => m.Sender)
                   .WithMany(m => m.MessagesSended)
                   .HasForeignKey(m => m.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);
           });             
        }
    }
}
