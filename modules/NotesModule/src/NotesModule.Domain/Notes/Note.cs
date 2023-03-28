using Volo.Abp.Identity;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace NotesModule.Notes
{
    public class Note : FullAuditedAggregateRoot<Guid>
    {
        [NotNull]
        public virtual string Content { get; set; }
        public Guid? IdentityUserId { get; set; }

        public Note()
        {

        }

        public Note(Guid id, Guid? identityUserId, string content)
        {

            Id = id;
            Check.NotNull(content, nameof(content));
            Content = content;
            IdentityUserId = identityUserId;
        }

    }
}