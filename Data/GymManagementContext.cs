using Microsoft.EntityFrameworkCore;
using GymManagement.Models;

namespace GymManagement.Data
{
    public class GymManagementContext : DbContext
    {
        public GymManagementContext(DbContextOptions<GymManagementContext> options)
            : base(options)
        {
        }

        // DbSet ??i di?n cho b?ng Member trong database
        public DbSet<Member> Members { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // C?u hình b?ng Member
            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.MembershipType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Price)
                    .HasPrecision(18, 2); // ??nh ngh?a ?? chính xác cho giá ti?n
            });

            // Seed d? li?u ban ??u (n?u c?n)
            modelBuilder.Entity<Member>().HasData(
                new Member { Id = 1, Name = "Tô Hoàng V?", Email = "hoangvu@gmail.com", Phone = "0123456789", MembershipType = "Gói 1 Tháng", Price = 500000 },
                new Member { Id = 2, Name = "Tô Hoàng Long", Email = "hoanglong@gmail.com", Phone = "0123456788", MembershipType = "Gói 1 N?m", Price = 4500000 }
            );
        }
    }
}
