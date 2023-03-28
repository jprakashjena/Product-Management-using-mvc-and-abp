using NotesModule.Notes;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.Identity;

namespace NotesModule.EntityFrameworkCore;

[ConnectionStringName(NotesModuleDbProperties.ConnectionStringName)]
public class NotesModuleDbContext : AbpDbContext<NotesModuleDbContext>, INotesModuleDbContext, IIdentityProDbContext
  
{
    public DbSet<Note> Notes { get; set; }

    public DbSet<IdentityUser> Users { get; set; }

    public DbSet<IdentityRole> Roles => throw new System.NotImplementedException();

    public DbSet<IdentityClaimType> ClaimTypes => throw new System.NotImplementedException();

    public DbSet<OrganizationUnit> OrganizationUnits => throw new System.NotImplementedException();

    public DbSet<IdentitySecurityLog> SecurityLogs => throw new System.NotImplementedException();

    public DbSet<IdentityLinkUser> LinkUsers => throw new System.NotImplementedException();


    /* Add DbSet for each Aggregate Root here. Example:
* public DbSet<Question> Questions { get; set; }
*/

    public NotesModuleDbContext(DbContextOptions<NotesModuleDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ConfigureNotesModule();
        builder.ConfigureIdentity();
        builder.ConfigureNotesModule();
        builder.Entity<IdentityUserLogin>()
    .HasKey(l => new { l.LoginProvider, l.ProviderKey });
    }
}