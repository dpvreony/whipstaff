using Microsoft.EntityFrameworkCore;

namespace Whipstaff.Testing.EntityFramework
{
    /// <summary>
    /// Fake Entity Framework DBContext.
    /// </summary>
    public sealed class FakeDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeDbContext"/> class.
        /// </summary>
        /// <param name="options">Entity Framework DB Context options for initializing instance.</param>
        public FakeDbContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// Gets or sets the Fake Add Audit Db Set.
        /// </summary>
        public DbSet<DbSets.FakeAddAuditDbSet> FakeAddAudit => Set<DbSets.FakeAddAuditDbSet>();

        /// <summary>
        /// Gets or sets the Fake Add Audit Pre Process Db Set.
        /// </summary>
        public DbSet<DbSets.FakeAddPreProcessAuditDbSet> FakeAddPreProcessAudit => Set<DbSets.FakeAddPreProcessAuditDbSet>();

        /// <summary>
        /// Gets or sets the Fake Add Audit Post Process Db Set.
        /// </summary>
        public DbSet<DbSets.FakeAddPostProcessAuditDbSet> FakeAddPostProcessAudit => Set<DbSets.FakeAddPostProcessAuditDbSet>();
    }
}
