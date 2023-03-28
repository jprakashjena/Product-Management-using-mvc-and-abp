using NotesModule.Shared;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using NotesModule.Notes;
using Volo.Abp.Content;
using NotesModule.Shared;

namespace NotesModule.Notes
{
    [RemoteService(Name = "NotesModule")]
    [Area("notesModule")]
    [ControllerName("Note")]
    [Route("api/notes-module/notes")]
    public class NoteController : AbpController, INotesAppService
    {
        private readonly INotesAppService _notesAppService;

        public NoteController(INotesAppService notesAppService)
        {
            _notesAppService = notesAppService;
        }

        [HttpGet]
        public Task<PagedResultDto<NoteWithNavigationPropertiesDto>> GetListAsync(GetNotesInput input)
        {
            return _notesAppService.GetListAsync(input);
        }

        [HttpGet]
        [Route("with-navigation-properties/{id}")]
        public Task<NoteWithNavigationPropertiesDto> GetWithNavigationPropertiesAsync(Guid id)
        {
            return _notesAppService.GetWithNavigationPropertiesAsync(id);
        }

        [HttpGet]
        [Route("{id}")]
        public virtual Task<NoteDto> GetAsync(Guid id)
        {
            return _notesAppService.GetAsync(id);
        }

        [HttpGet]
        [Route("identity-user-lookup")]
        public Task<PagedResultDto<LookupDto<Guid>>> GetIdentityUserLookupAsync(LookupRequestDto input)
        {
            return _notesAppService.GetIdentityUserLookupAsync(input);
        }

        [HttpPost]
        public virtual Task<NoteDto> CreateAsync(NoteCreateDto input)
        {
            return _notesAppService.CreateAsync(input);
        }

        [HttpPut]
        [Route("{id}")]
        public virtual Task<NoteDto> UpdateAsync(Guid id, NoteUpdateDto input)
        {
            return _notesAppService.UpdateAsync(id, input);
        }

        [HttpDelete]
        [Route("{id}")]
        public virtual Task DeleteAsync(Guid id)
        {
            return _notesAppService.DeleteAsync(id);
        }

        [HttpGet]
        [Route("as-excel-file")]
        public virtual Task<IRemoteStreamContent> GetListAsExcelFileAsync(NoteExcelDownloadDto input)
        {
            return _notesAppService.GetListAsExcelFileAsync(input);
        }

        [HttpGet]
        [Route("download-token")]
        public Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            return _notesAppService.GetDownloadTokenAsync();
        }
    }
}