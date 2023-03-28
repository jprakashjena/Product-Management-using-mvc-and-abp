using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace NotesModule.Notes
{
    public class NoteCreateDto
    {
        [Required]
        [StringLength(int.MaxValue, MinimumLength = NoteConsts.ContentMinLength)]
        public string Content { get; set; }
        public Guid? IdentityUserId { get; set; }
    }
}