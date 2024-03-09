using Microsoft.EntityFrameworkCore;

namespace WebSignalR.Models
{
    public partial class WebSignalRContext : DbContext
    {
        public WebSignalRContext() { }

        public WebSignalRContext(DbContextOptions<WebSignalRContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Group> Groups { get; set; } = null!;
        public virtual DbSet<Message> Messages { get; set; } = null!;
        public virtual DbSet<GroupUser> GroupUsers { get; set; } = null!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                string connectionString = GetConnectionString();
                optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            }
        }

        private string GetConnectionString()
        {
            IConfiguration config = new ConfigurationBuilder()

            .SetBasePath(Directory.GetCurrentDirectory())

            .AddJsonFile("appsettings.json", true, true)

            .Build();

            var strConn = config["ConnectionStrings:MySQL"];

            return strConn;

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.UserId);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .HasMaxLength(255);

                entity.Property(e => e.Role)
                    .HasMaxLength(255);

                entity.Property(e => e.Status)
                    .HasMaxLength(255);

                entity.Property(e => e.Username)
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(e => e.GroupId);

                entity.ToTable("Group");

                entity.Property(e => e.GroupId);

                entity.Property(e => e.GroupName);

                entity.Property(e => e.NumOfMember);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.MessageId);

                entity.ToTable("Message");

                entity.Property(e => e.MessageId)
                    .HasColumnName("MessageID")
                    .HasColumnType("int")
                    .UseMySqlIdentityColumn();

                entity.Property(e => e.Content);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Group)
                    .WithMany()
                    .HasForeignKey(e => e.GroupId)
                    .OnDelete(DeleteBehavior.Cascade);

            });

            modelBuilder.Entity<GroupUser>(entity =>
            {
                entity.HasKey(e => new { e.GroupId, e.UserId });

                entity.ToTable("GroupUser");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Group)
                    .WithMany()
                    .HasForeignKey(e => e.GroupId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
