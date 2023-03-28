using Volo.Abp.EntityFrameworkCore.Modeling;
using NotesModule.Notes;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Identity;

namespace NotesModule.EntityFrameworkCore;

public static class NotesModuleDbContextModelCreatingExtensions
{
    public static void ConfigureNotesModule(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(NotesModuleDbProperties.DbTablePrefix + "Questions", NotesModuleDbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {

        }
        if (builder.IsHostDatabase())
        {
            builder.Entity<Note>(b =>
{
    b.ToTable(NotesModuleDbProperties.DbTablePrefix + "Notes", NotesModuleDbProperties.DbSchema);
    b.ConfigureByConvention();
    b.Property(x => x.Content).HasColumnName(nameof(Note.Content)).IsRequired();
    b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.IdentityUserId).OnDelete(DeleteBehavior.NoAction);
});

        }
    }
}