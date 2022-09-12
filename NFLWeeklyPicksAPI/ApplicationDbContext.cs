using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NFLWeeklyPicksAPI.Models.Entities;
using NFLWeeklyPicksAPI.Models.Entities.Configuration;

namespace NFLWeeklyPicksAPI
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new RoleConfiguration());
        }

        public DbSet<UserPicks> UserPicks { get; set; }
        public DbSet<UserPickLineItems> UserPickLineItems { get; set; }
        public DbSet<PickTypes> PickTypes { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<SeasonWeeks> SeasonWeeks { get; set; }
        public DbSet<Competitions> Competitions { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<TeamSeasonRecords> TeamSeasonRecords { get; set; }
    }
}