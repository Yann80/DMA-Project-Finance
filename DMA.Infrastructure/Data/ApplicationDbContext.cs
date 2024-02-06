using Core.DTO;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DMA.Infrastructure.Data
{
    public class ApplicationDbContext: IdentityDbContext<BackOfficeUser>
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<TaskItem> TaskItems { get; set; }
        public virtual DbSet<ProjectTask> ProjectTasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BackOfficeUser>(b =>
            {
                b.HasKey(user => user.Id);
                b.ToTable("Users");
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.HasKey(claim => claim.Id);
                b.ToTable("UserClaims");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.HasKey(login => login.UserId);
                b.ToTable("UserLogins");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(b =>
            {
                b.HasKey(userToken => userToken.UserId);
                b.ToTable("UserTokens");
            });

            modelBuilder.Entity<IdentityRole>(b =>
            {
                b.HasKey(role => role.Id);
                b.ToTable("Roles");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.HasKey(roleClaim => roleClaim.Id);
                b.ToTable("RoleClaims");
            });

            modelBuilder.Entity<IdentityUserRole<string>>(b =>
            {
                b.HasKey(roleClaim => new { roleClaim.UserId, roleClaim.RoleId });
                b.ToTable("UserRoles");
            });

            modelBuilder.Entity<TaskItem>()
                .HasKey("TaskItemId");

            modelBuilder.Entity<ProjectTask>()
                .Property(pt => pt.Billing)
                .HasColumnType("decimal")
                .HasPrecision(18,2);
        }
    }
}
