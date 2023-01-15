using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolChat.Models;
using SchoolChatOriginal.Models;

namespace SchoolChatOriginal.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SchoolGroup> Groups { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<GroupRequest> GroupRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // definire primary key compus
            modelBuilder.Entity<UserGroup>()
                .HasKey(ab => new { ab.Id, ab.IdUser, ab.IdGroup });

            // definire relatii cu modelele Bookmark si Article (FK)
            modelBuilder.Entity<UserGroup>()
                .HasOne(ab => ab.User)
                .WithMany(ab => ab.UserGroups)
                .HasForeignKey(ab => ab.IdUser);

            modelBuilder.Entity<UserGroup>()
                .HasOne(ab => ab.Group)
                .WithMany(ab => ab.UserGroups)
                .HasForeignKey(ab => ab.IdGroup);

            modelBuilder.Entity<GroupRequest>()
                .HasKey(ab => new { ab.Id, ab.IdUser, ab.IdGroup });

            // definire relatii cu modelele Bookmark si Article (FK)
            modelBuilder.Entity<GroupRequest>()
                .HasOne(ab => ab.User)
                .WithMany(ab => ab.GroupRequests)
                .HasForeignKey(ab => ab.IdUser);

            modelBuilder.Entity<GroupRequest>()
                .HasOne(ab => ab.Group)
                .WithMany(ab => ab.GroupRequests)
                .HasForeignKey(ab => ab.IdGroup);
        }
    }


}