using VBMS.Application.Interfaces.Services;
using VBMS.Application.Models.Chat;
using VBMS.Infrastructure.Models.Identity;
using VBMS.Domain.Contracts;
using VBMS.Domain.Entities.Catalog;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Domain.Entities.ExtendedAttributes;
using VBMS.Domain.Entities.Misc;
using VBMS.Domain.Entities.vbms.bookings;
using VBMS.Domain.Entities.vbms.diary;
using VBMS.Domain.Entities.vbms.partners;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Infrastructure.Contexts
{
    public class BlazorHeroContext : AuditableContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;

        public BlazorHeroContext(DbContextOptions<BlazorHeroContext> options, ICurrentUserService currentUserService, IDateTimeService dateTimeService)
            : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public DbSet<ChatHistory<BlazorHeroUser>> ChatHistories { get; set; }
        public DbSet<ProductTest> ProductTests { get; set; }
        public DbSet<BrandTest> BrandTests { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DocumentExtendedAttribute> DocumentExtendedAttributes { get; set; }


        public DbSet<Booking> Bookings { get; set; }
        public DbSet<BookingAttribute> BookingAttributes { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Diary> Diarys { get; set; }
        public DbSet<DiaryType> DiaryTypes { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<PartnerAttribute> PartnerAttributes { get; set; }
        public DbSet<PartnerType> PartnerTypes { get; set; }
        public DbSet<PartnerTypeAttribute> PartnerTypeAttributes { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleAttribute> VehicleAttributes { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<VehicleTypeAttribute> VehicleTypeAttributes { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTimeService.NowUtc;
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = _dateTimeService.NowUtc;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        break;
                }
            }
            if (_currentUserService.UserId == null)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await base.SaveChangesAsync(_currentUserService.UserId, cancellationToken);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }
            base.OnModelCreating(builder);
            builder.Entity<ChatHistory<BlazorHeroUser>>(entity =>
            {
                entity.ToTable("ChatHistory");

                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.ChatHistoryFromUsers)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ChatHistoryToUsers)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            builder.Entity<BlazorHeroUser>(entity =>
            {
                entity.ToTable(name: "Users", "Identity");
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            builder.Entity<BlazorHeroRole>(entity =>
            {
                entity.ToTable(name: "Roles", "Identity");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles", "Identity");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims", "Identity");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins", "Identity");
            });

            builder.Entity<BlazorHeroRoleClaim>(entity =>
            {
                entity.ToTable(name: "RoleClaims", "Identity");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.RoleClaims)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens", "Identity");
            });
        }
    }
}