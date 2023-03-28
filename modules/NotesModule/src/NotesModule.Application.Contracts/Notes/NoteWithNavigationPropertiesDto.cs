using Volo.Abp.Identity;

using System;
using Volo.Abp.Application.Dtos;
using System.Collections.Generic;

namespace NotesModule.Notes
{
    public class NoteWithNavigationPropertiesDto
    {
        public NoteDto Note { get; set; }

        public IdentityUserDto IdentityUser { get; set; }

    }
}