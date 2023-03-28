using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.TagHelpers.Form;
using NotesModule.Notes;
using NotesModule.Shared;

namespace NotesModule.Web.Pages.NotesModule.Notes
{
    public class IndexModel : AbpPageModel
    {
        public string ContentFilter { get; set; }
        [SelectItems(nameof(IdentityUserLookupList))]
        public Guid? IdentityUserIdFilter { get; set; }
        public List<SelectListItem> IdentityUserLookupList { get; set; } = new List<SelectListItem>
        {
            new SelectListItem(string.Empty, "")
        };

        private readonly INotesAppService _notesAppService;

        public IndexModel(INotesAppService notesAppService)
        {
            _notesAppService = notesAppService;
        }

        public async Task OnGetAsync()
        {
            IdentityUserLookupList.AddRange((
                    await _notesAppService.GetIdentityUserLookupAsync(new LookupRequestDto
                    {
                        MaxResultCount = LimitedResultRequestDto.MaxMaxResultCount
                    })).Items.Select(t => new SelectListItem(t.DisplayName, t.Id.ToString())).ToList()
            );

            await Task.CompletedTask;
        }
    }
}