namespace NotesModule1.EntityFrameworkCore;

/* This class can be used as a base class for EF Core integration tests,
 * while SampleRepository_Tests uses a different approach.
 */
public abstract class NotesModule1EntityFrameworkCoreTestBase : NotesModule1TestBase<NotesModule1EntityFrameworkCoreTestModule>
{

}
