using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace NotesModule1.EntityFrameworkCore;

public static class NotesModule1DbContextModelCreatingExtensions
{
    public static void ConfigureNotesModule1(
        this ModelBuilder builder)
    {
        Check.NotNull(builder, nameof(builder));

        /* Configure all entities here. Example:

        builder.Entity<Question>(b =>
        {
            //Configure table & schema name
            b.ToTable(NotesModule1DbProperties.DbTablePrefix + "Questions", NotesModule1DbProperties.DbSchema);

            b.ConfigureByConvention();

            //Properties
            b.Property(q => q.Title).IsRequired().HasMaxLength(QuestionConsts.MaxTitleLength);

            //Relations
            b.HasMany(question => question.Tags).WithOne().HasForeignKey(qt => qt.QuestionId);

            //Indexes
            b.HasIndex(q => q.CreationTime);
        });
        */
    }
}
