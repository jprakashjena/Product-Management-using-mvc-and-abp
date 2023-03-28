using NotesModule.Shared;
using Volo.Abp.Identity;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using NotesModule.Permissions;
using NotesModule.Notes;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using NotesModule.Shared;

namespace NotesModule.Notes
{

    [Authorize(NotesModulePermissions.Notes.Default)]
    public class NotesAppService : ApplicationService, INotesAppService
    {
        private readonly IDistributedCache<NoteExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly INoteRepository _noteRepository;
        private readonly NoteManager _noteManager;
        private readonly IRepository<IdentityUser, Guid> _identityUserRepository;

        public NotesAppService(INoteRepository noteRepository, NoteManager noteManager, IDistributedCache<NoteExcelDownloadTokenCacheItem, string> excelDownloadTokenCache, IRepository<IdentityUser, Guid> identityUserRepository)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _noteRepository = noteRepository;
            _noteManager = noteManager; _identityUserRepository = identityUserRepository;
        }

        public virtual async Task<PagedResultDto<NoteWithNavigationPropertiesDto>> GetListAsync(GetNotesInput input)
        {
            var totalCount = await _noteRepository.GetCountAsync(input.FilterText, input.Content, input.IdentityUserId);
            var items = await _noteRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Content, input.IdentityUserId, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<NoteWithNavigationPropertiesDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<NoteWithNavigationProperties>, List<NoteWithNavigationPropertiesDto>>(items)
            };
        }

        public virtual async Task<NoteWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return ObjectMapper.Map<NoteWithNavigationProperties, NoteWithNavigationPropertiesDto>
                (await _noteRepository.GetWithNavigationPropertiesAsync(id));
        }

        public virtual async Task<NoteDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Note, NoteDto>(await _noteRepository.GetAsync(id));
        }

        public virtual async Task<PagedResultDto<LookupDto<Guid>>> GetIdentityUserLookupAsync(LookupRequestDto input)
        {
            var query = (await _identityUserRepository.GetQueryableAsync())
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    x => x.UserName != null &&
                         x.UserName.Contains(input.Filter));

            var lookupData = await query.PageBy(input.SkipCount, input.MaxResultCount).ToDynamicListAsync<IdentityUser>();
            var totalCount = query.Count();
            return new PagedResultDto<LookupDto<Guid>>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<IdentityUser>, List<LookupDto<Guid>>>(lookupData)
            };
        }

        [Authorize(NotesModulePermissions.Notes.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _noteRepository.DeleteAsync(id);
        }

        [Authorize(NotesModulePermissions.Notes.Create)]
        public virtual async Task<NoteDto> CreateAsync(NoteCreateDto input)
        {

            var note = await _noteManager.CreateAsync(
            input.IdentityUserId, input.Content
            );

            return ObjectMapper.Map<Note, NoteDto>(note);
        }

        [Authorize(NotesModulePermissions.Notes.Edit)]
        public virtual async Task<NoteDto> UpdateAsync(Guid id, NoteUpdateDto input)
        {

            var note = await _noteManager.UpdateAsync(
            id,
            input.IdentityUserId, input.Content, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Note, NoteDto>(note);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(NoteExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var notes = await _noteRepository.GetListWithNavigationPropertiesAsync(input.FilterText, input.Content);
            var items = notes.Select(item => new
            {
                Content = item.Note.Content,

                IdentityUserUserName = item.IdentityUser?.UserName,

            });

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(items);
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Notes.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new NoteExcelDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}