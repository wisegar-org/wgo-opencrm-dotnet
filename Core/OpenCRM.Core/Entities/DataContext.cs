    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OpenCRM.Core.Data;


namespace OpenCRM.Core
{
    public class DataContext
        : IdentityDbContext<UserEntity, RoleEntity, Guid,
        UserClaimEntity, UserRoleEntity, UserLoginEntity,
        RoleClaimEntity, UserTokenEntity>
    {
        public DataContext(DbContextOptions options)
      : base(options)
        {
            ChangeTracker.StateChanged += ChangeTracker_StateChanged;
            ChangeTracker.Tracked += ChangeTracker_StateChanged;
        }
        
        public DataContext(DbSet<LanguageEntity> languagess)
        {
            Languagess = languagess;
        }

        public DataContext(DbSet<TranslationEntity> translationss)
        {
            Translationss = translationss;
        }

        private void ChangeTracker_StateChanged(object? sender, EntityEntryEventArgs e)
        {
            if (e.Entry.Entity is IHasTimestamps entityWithTimestamps)
            {
                switch (e.Entry.State)
                {
                    case EntityState.Deleted:
                        entityWithTimestamps.DeletedAt = DateTime.UtcNow;
                        Console.WriteLine($"Stamped for delete: {e.Entry.Entity}");
                        break;
                    case EntityState.Modified:
                        entityWithTimestamps.UpdatedAt = DateTime.UtcNow;
                        Console.WriteLine($"Stamped for update: {e.Entry.Entity}");
                        break;
                    case EntityState.Added:
                        entityWithTimestamps.AddedAt = DateTime.UtcNow;
                        Console.WriteLine($"Stamped for insert: {e.Entry.Entity}");
                        break;
                }
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserEntity>().ToTable("Users");
            builder.Entity<UserClaimEntity>().ToTable("UserClaims");
            builder.Entity<UserLoginEntity>().ToTable("UserLogins");
            builder.Entity<UserRoleEntity>().ToTable("UserRoles");
            builder.Entity<UserTokenEntity>().ToTable("UserTokens");
            builder.Entity<UserSessionEntity>().ToTable("UserSessions");

            builder.Entity<RoleEntity>().ToTable("Roles");
            builder.Entity<RoleClaimEntity>().ToTable("RoleClaims");

            builder.Entity<MediaEntity>().ToTable("Medias");
            builder.Entity<HistoryEntity>().ToTable("History");
            builder.Entity<DataBlockEntity>().ToTable("DataBlocks");
            builder.Entity<DataContainerEntity>().ToTable("DataContainers");
            builder.Entity<LanguageEntity>().ToTable("Languages");
            builder.Entity<TranslationEntity>((entity) =>
            {
                entity.ToTable("Translations");
               // entity.HasIndex(e => e.Key).IsUnique();
            });
        }
        
        public DbSet<MediaEntity> Medias { get; set; }
        public DbSet<HistoryEntity> History { get; set; }
        public DbSet<DataBlockEntity> DataBlocks { get; set; }
        public DbSet<LanguageEntity> Languagess { get; set; }
        public DbSet<TranslationEntity> Translationss { get; set; }
        public DbSet<DataContainerEntity> DataContainers { get; set; }
        public DbSet<UserSessionEntity> UserSessions { get; set; }

    }
}