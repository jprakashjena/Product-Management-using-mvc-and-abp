using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using NotesModule.Notes;
using NotesModule.EntityFrameworkCore;
using Xunit;

namespace NotesModule.Notes
{
    public class NoteRepositoryTests : NotesModuleEntityFrameworkCoreTestBase
    {
        private readonly INoteRepository _noteRepository;

        public NoteRepositoryTests()
        {
            _noteRepository = GetRequiredService<INoteRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _noteRepository.GetListAsync(
                    content: "0"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("3297aba2-d657-4895-99b9-c9d812ab3057"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _noteRepository.GetCountAsync(
                    content: "3"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}