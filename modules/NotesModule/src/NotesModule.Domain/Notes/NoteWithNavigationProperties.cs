using Volo.Abp.Identity;

using System;
using System.Collections.Generic;

namespace NotesModule.Notes
{
    public class NoteWithNavigationProperties
    {
        public Note Note { get; set; }

        public IdentityUser IdentityUser { get; set; }
        

        
    }
}