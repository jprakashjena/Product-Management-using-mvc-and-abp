using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace NotesModule.Notes
{
    public interface INoteRepository : IRepository<Note, Guid>
    {
        Task<NoteWithNavigationProperties> GetWithNavigationPropertiesAsync(
    Guid id,
    CancellationToken cancellationToken = default
);

        Task<List<NoteWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string filterText = null,
            string content = null,
            Guid? identityUserId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default
        );

        Task<List<Note>> GetListAsync(
                    string filterText = null,
                    string content = null,
                    string sorting = null,
                    int maxResultCount = int.MaxValue,
                    int skipCount = 0,
                    CancellationToken cancellationToken = default
                );

        Task<long> GetCountAsync(
            string filterText = null,
            string content = null,
            Guid? identityUserId = null,
            CancellationToken cancellationToken = default);
    }
}