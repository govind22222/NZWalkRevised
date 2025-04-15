using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NZWalkRevise.Database
{
    public class NzAuthDbContext : IdentityDbContext
    {

        public NzAuthDbContext(DbContextOptions<NzAuthDbContext> options) : base(options)
        {

        }
    }
}
