using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using NotesModule.EntityFrameworkCore;

namespace NotesModule.Notes
{
    public class EfCoreNoteRepository : EfCoreRepository<NotesModuleDbContext, Note, Guid>, INoteRepository
    {
        public EfCoreNoteRepository(IDbContextProvider<NotesModuleDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<NoteWithNavigationProperties> GetWithNavigationPropertiesAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();

            return (await GetDbSetAsync()).Where(b => b.Id == id)
                .Select(note => new NoteWithNavigationProperties
                {
                    Note = note,
                    IdentityUser = dbContext.Users.FirstOrDefault(c => c.Id == note.IdentityUserId)
                }).FirstOrDefault();
        }

        public async Task<List<NoteWithNavigationProperties>> GetListWithNavigationPropertiesAsync(
            string filterText = null,
            string content = null,
            Guid? identityUserId = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, content, identityUserId);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? NoteConsts.GetDefaultSorting(true) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        protected virtual async Task<IQueryable<NoteWithNavigationProperties>> GetQueryForNavigationPropertiesAsync()
        {
            return from note in (await GetDbSetAsync())
                   join identityUser in (await GetDbContextAsync()).Users on note.IdentityUserId equals identityUser.Id into identityUsers
                   from identityUser in identityUsers.DefaultIfEmpty()

                   select new NoteWithNavigationProperties
                   {
                       Note = note,
                       IdentityUser = identityUser
                   };
        }

        protected virtual IQueryable<NoteWithNavigationProperties> ApplyFilter(
            IQueryable<NoteWithNavigationProperties> query,
            string filterText,
            string content = null,
            Guid? identityUserId = null)
        {
            return query
                .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Note.Content.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(content), e => e.Note.Content.Contains(content))
                    .WhereIf(identityUserId != null && identityUserId != Guid.Empty, e => e.IdentityUser != null && e.IdentityUser.Id == identityUserId);
        }

        public async Task<List<Note>> GetListAsync(
            string filterText = null,
            string content = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, content);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? NoteConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            string content = null,
            Guid? identityUserId = null,
            CancellationToken cancellationToken = default)
        {
            var query = await GetQueryForNavigationPropertiesAsync();
            query = ApplyFilter(query, filterText, content, identityUserId);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Note> ApplyFilter(
            IQueryable<Note> query,
            string filterText,
            string content = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.Content.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(content), e => e.Content.Contains(content));
        }
    }
}