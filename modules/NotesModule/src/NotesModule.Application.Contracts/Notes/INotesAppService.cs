using NotesModule.Shared;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Content;
using NotesModule.Shared;

namespace NotesModule.Notes
{
    public interface INotesAppService : IApplicationService
    {
        Task<PagedResultDto<NoteWithNavigationPropertiesDto>> GetListAsync(GetNotesInput input);

        Task<NoteWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id);

        Task<NoteDto> GetAsync(Guid id);

        Task<PagedResultDto<LookupDto<Guid>>> GetIdentityUserLookupAsync(LookupRequestDto input);

        Task DeleteAsync(Guid id);

        Task<NoteDto> CreateAsync(NoteCreateDto input);

        Task<NoteDto> UpdateAsync(Guid id, NoteUpdateDto input);

        Task<IRemoteStreamContent> GetListAsExcelFileAsync(NoteExcelDownloadDto input);

        Task<DownloadTokenResultDto> GetDownloadTokenAsync();
    }
}