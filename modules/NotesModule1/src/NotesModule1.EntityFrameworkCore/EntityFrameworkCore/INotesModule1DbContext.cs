using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NotesModule1.EntityFrameworkCore;

[ConnectionStringName(NotesModule1DbProperties.ConnectionStringName)]
public interface INotesModule1DbContext : IEfCoreDbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}
