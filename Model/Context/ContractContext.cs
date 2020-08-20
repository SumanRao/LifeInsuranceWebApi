using Microsoft.EntityFrameworkCore;

namespace LifeInsurance
{
    public class ContractContext : DbContext
    {
        public ContractContext(DbContextOptions<ContractContext> options)
            : base(options)
        {
        }

        public DbSet<Contract> Contracts { get; set; }

        public DbSet<RateChart> RateCharts { get; set; }
        public DbSet<CoveragePlans> CoveragePlans { get; set; }


    }

}