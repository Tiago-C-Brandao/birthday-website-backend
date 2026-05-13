using BirthdayWebsiteAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BirthdayWebsiteAPI.Data
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Guest> Guests { get; set; }
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            List<IdentityRole> roles = new List<IdentityRole>
            {
                new IdentityRole { Id = "3f5d6a9e-1c2b-4d5e-9f6a-0a1b2c3d4e5f", Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = "a1b2c3d4-0000-0000-0000-000000000001" },
                new IdentityRole { Id = "7a8b9c0d-2e3f-4a5b-8c9d-1e2f3a4b5c6d", Name = "User", NormalizedName = "USER", ConcurrencyStamp = "b2c3d4e5-0000-0000-0000-000000000002" }
            };
            modelBuilder.Entity<IdentityRole>().HasData(roles);

            modelBuilder.Entity<Gift>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Guest>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Delete related Guest records when User is deleted

            modelBuilder.Entity<Guest>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(g => g.AccompanyingBy)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }

    public static class AppDbContextSeed
    {
        public static async Task SeedAdminUserAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            var role = await roleManager.FindByNameAsync("Admin");

            if (role == null)
            {
                role = new IdentityRole("Admin");
                await roleManager.CreateAsync(role);
            }

            var adminSection = configuration.GetSection("AdminUser");


            var adminUser = await userManager.FindByEmailAsync(adminSection["Email"]);

            if (adminUser == null)
            {
                adminUser = new User
                {
                    UserName = adminSection["UserName"],
                    Email = adminSection["Email"],
                    FullName = adminSection["FullName"],
                    WhatsApp = adminSection["Whatsapp"],
                    CreatedAt = DateTime.UtcNow
                };

                await userManager.CreateAsync(adminUser, adminSection["Password"]); // Set a default password
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
