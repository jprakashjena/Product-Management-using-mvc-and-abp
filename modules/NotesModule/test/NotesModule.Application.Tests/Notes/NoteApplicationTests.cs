using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace NotesModule.Notes
{
    public class NotesAppServiceTests : NotesModuleApplicationTestBase
    {
        private readonly INotesAppService _notesAppService;
        private readonly IRepository<Note, Guid> _noteRepository;

        public NotesAppServiceTests()
        {
            _notesAppService = GetRequiredService<INotesAppService>();
            _noteRepository = GetRequiredService<IRepository<Note, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _notesAppService.GetListAsync(new GetNotesInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Note.Id == Guid.Parse("3297aba2-d657-4895-99b9-c9d812ab3057")).ShouldBe(true);
            result.Items.Any(x => x.Note.Id == Guid.Parse("2c97dc25-a964-4f3b-bcdd-05bb3155b73a")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _notesAppService.GetAsync(Guid.Parse("3297aba2-d657-4895-99b9-c9d812ab3057"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("3297aba2-d657-4895-99b9-c9d812ab3057"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new NoteCreateDto
            {
                Content = "c"
            };

            // Act
            var serviceResult = await _notesAppService.CreateAsync(input);

            // Assert
            var result = await _noteRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Content.ShouldBe("c");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new NoteUpdateDto()
            {
                Content = "e"
            };

            // Act
            var serviceResult = await _notesAppService.UpdateAsync(Guid.Parse("3297aba2-d657-4895-99b9-c9d812ab3057"), input);

            // Assert
            var result = await _noteRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Content.ShouldBe("e");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _notesAppService.DeleteAsync(Guid.Parse("3297aba2-d657-4895-99b9-c9d812ab3057"));

            // Assert
            var result = await _noteRepository.FindAsync(c => c.Id == Guid.Parse("3297aba2-d657-4895-99b9-c9d812ab3057"));

            result.ShouldBeNull();
        }
    }
}