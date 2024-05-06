using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using upload.Models;

namespace upload.Data;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options) {
    public override DbSet<User> Users { get; set; } = default!;
    public DbSet<StoredFile> StoredFiles { get; set; } = default!;
    public DbSet<Thumbnail> Thumbnails { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);

        builder.Entity<User>().ToTable("Users");
        builder.Entity<User>().Property(u => u.Id).ValueGeneratedOnAdd();
        builder.Entity<StoredFile>().ToTable("Files");
        builder.Entity<StoredFile>().Property(g => g.CreatedAt).HasDefaultValue(DateTime.MinValue);

        builder.Entity<StoredFile>().HasOne(p => p.Author).WithMany(u => u.StoredFiles).HasForeignKey(p => p.AuthorId).OnDelete(DeleteBehavior.Cascade);
        builder.Entity<Thumbnail>().HasOne(p => p.StoredFile).WithMany(u => u.Thumbnails).HasForeignKey(p => p.StoredFileId).OnDelete(DeleteBehavior.Cascade);

        Guid adminId = new("11111111-1111-1111-1111-111111111111");

        builder.Entity<IdentityRole<Guid>>().HasData(new IdentityRole<Guid> {
            Id = adminId,
            Name = "admin",
            NormalizedName = "ADMIN",
        });

        builder.Entity<IdentityRole<Guid>>().HasData(new IdentityRole<Guid> {
            Id = Guid.NewGuid(),
            Name = "moderator",
            NormalizedName = "MODERATOR",
        });

        builder.Entity<User>(entity => {
            PasswordHasher<User> hasher = new PasswordHasher<User>();
            entity.HasData(new User {
                Id = adminId,
                FullName = "Administrator User",
                UserName = "admin@local.slhn.cz",
                NormalizedUserName = "ADMIN@LOCAL.SLHN.CZ",
                Email = "admin@local.slhn.cz",
                NormalizedEmail = "ADMIN@LOCAL.SLHN.CZ",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(new User(), "admin"),
                SecurityStamp = "Asdfiasjfisda",
            });
        });

        builder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid> {
            RoleId = adminId,
            UserId = adminId,
        });
    }
}
