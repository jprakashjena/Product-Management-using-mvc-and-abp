using NotesModule.Notes;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace NotesModule.EntityFrameworkCore;

[ConnectionStringName(NotesModuleDbProperties.ConnectionStringName)]
public interface INotesModuleDbContext : IEfCoreDbContext
{
    DbSet<Note> Notes { get; set; }
    /* Add DbSet for each Aggregate Root here. Example:
     * DbSet<Question> Questions { get; }
     */
}