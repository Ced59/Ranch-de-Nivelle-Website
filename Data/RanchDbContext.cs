using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RanchDuBonheur.Data
{
    public class RanchDbContext : IdentityDbContext
    {
        public RanchDbContext(DbContextOptions<RanchDbContext> options)
            : base(options)
        {
        }
    }
}
