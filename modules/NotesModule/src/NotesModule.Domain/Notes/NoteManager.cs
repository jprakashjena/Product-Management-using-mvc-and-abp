using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace NotesModule.Notes
{
    public class NoteManager : DomainService
    {
        private readonly INoteRepository _noteRepository;

        public NoteManager(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<Note> CreateAsync(
        Guid? identityUserId, string content)
        {
            Check.NotNullOrWhiteSpace(content, nameof(content));
            Check.Length(content, nameof(content), int.MaxValue, NoteConsts.ContentMinLength);

            var note = new Note(
             GuidGenerator.Create(),
             identityUserId, content
             );

            return await _noteRepository.InsertAsync(note);
        }

        public async Task<Note> UpdateAsync(
            Guid id,
            Guid? identityUserId, string content, [CanBeNull] string concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(content, nameof(content));
            Check.Length(content, nameof(content), int.MaxValue, NoteConsts.ContentMinLength);

            var note = await _noteRepository.GetAsync(id);

            note.IdentityUserId = identityUserId;
            note.Content = content;

            note.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _noteRepository.UpdateAsync(note);
        }

    }
}