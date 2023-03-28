using Volo.Abp.Application.Dtos;
using System;

namespace NotesModule.Notes
{
    public class NoteExcelDownloadDto
    {
        public string DownloadToken { get; set; }

        public string FilterText { get; set; }

        public string Content { get; set; }
        public Guid? IdentityUserId { get; set; }

        public NoteExcelDownloadDto()
        {

        }
    }
}