using Volo.Abp.Application.Dtos;
using System;

namespace NotesModule.Notes
{
    public class GetNotesInput : PagedAndSortedResultRequestDto
    {
        public string FilterText { get; set; }

        public string Content { get; set; }
        public Guid? IdentityUserId { get; set; }

        public GetNotesInput()
        {

        }
    }
}