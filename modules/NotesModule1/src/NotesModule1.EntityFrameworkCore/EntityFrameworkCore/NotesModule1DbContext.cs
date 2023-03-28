using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NotesModule1.EntityFrameworkCore;

[ConnectionStringName(NotesModule1DbProperties.ConnectionStringName)]
public class NotesModule1DbContext : AbpDbContext<NotesModule1DbContext>, INotesModule1DbContext
{
    /* Add DbSet for each Aggregate Root here. Example:
     * public DbSet<Question> Questions { get; set; }
     */

    public NotesModule1DbContext(DbContextOptions<NotesModule1DbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureNotesModule1();
    }
}
