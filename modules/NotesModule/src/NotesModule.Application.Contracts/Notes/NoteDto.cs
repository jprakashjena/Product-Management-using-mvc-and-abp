using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace NotesModule.Notes
{
    public class NoteDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string Content { get; set; }
        public Guid? IdentityUserId { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}