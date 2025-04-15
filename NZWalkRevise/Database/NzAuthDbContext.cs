using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalkRevise.Database
{
    public class NzAuthDbContext : IdentityDbContext
    {

        public NzAuthDbContext(DbContextOptions<NzAuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            string roleRead = "re871153-ecae-40d8-a2c1-91616ace5805";
            string roleWrite = "w11acb22-86f4-46cf-817c-fdbc4a39a323";
            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = roleRead,
                    ConcurrencyStamp = roleRead,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper()

                },
                new IdentityRole
                {
                    Id = roleWrite,
                    ConcurrencyStamp = roleWrite,
                    Name = "Writer",
                    NormalizedName = "Writter".ToUpper()
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
