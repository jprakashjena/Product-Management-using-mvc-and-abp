using NotesModule.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NotesModule.Notes;
using Volo.Abp.Users;

namespace NotesModule.Web.Pages.NotesModule.Notes
{
    public class CreateModalModel : NotesModulePageModel
    {
        [BindProperty]
        public NoteCreateViewModel Note { get; set; }

        public List<SelectListItem> IdentityUserLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem(" — ", "")
        };

        private readonly INotesAppService _notesAppService;
        private readonly ICurrentUser _currentUserService;

        public CreateModalModel(INotesAppService notesAppService, ICurrentUser currentUserService)
        {
            _notesAppService = notesAppService;
            _currentUserService = currentUserService;
        }

        public async Task OnGetAsync()
        {
            Note = new NoteCreateViewModel();
            IdentityUserLookupList.AddRange((
                                    await _notesAppService.GetIdentityUserLookupAsync(new LookupRequestDto
                                    {
                                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
                        );
            IdentityUserLookupList.Where(s => s.Value == _currentUserService.Id.ToString()).First().Selected = true;
            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            await _notesAppService.CreateAsync(ObjectMapper.Map<NoteCreateViewModel, NoteCreateDto>(Note));
            return NoContent();
        }
    }

    public class NoteCreateViewModel : NoteCreateDto
    {
    }
}