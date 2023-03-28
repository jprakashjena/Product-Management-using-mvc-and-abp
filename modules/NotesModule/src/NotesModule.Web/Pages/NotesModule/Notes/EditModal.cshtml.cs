using NotesModule.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using NotesModule.Notes;

namespace NotesModule.Web.Pages.NotesModule.Notes
{
    public class EditModalModel : NotesModulePageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public NoteUpdateViewModel Note { get; set; }

        public List<SelectListItem> IdentityUserLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem(" — ", "")
        };

        private readonly INotesAppService _notesAppService;

        public EditModalModel(INotesAppService notesAppService)
        {
            _notesAppService = notesAppService;
        }

        public async Task OnGetAsync()
        {
            var noteWithNavigationPropertiesDto = await _notesAppService.GetWithNavigationPropertiesAsync(Id);
            Note = ObjectMapper.Map<NoteDto, NoteUpdateViewModel>(noteWithNavigationPropertiesDto.Note);

            IdentityUserLookupList.AddRange((
                                    await _notesAppService.GetIdentityUserLookupAsync(new LookupRequestDto
                                    {
                                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                        );

        }

        public async Task<NoContentResult> OnPostAsync()
        {

            await _notesAppService.UpdateAsync(Id, ObjectMapper.Map<NoteUpdateViewModel, NoteUpdateDto>(Note));
            return NoContent();
        }
    }

    public class NoteUpdateViewModel : NoteUpdateDto
    {
    }
}