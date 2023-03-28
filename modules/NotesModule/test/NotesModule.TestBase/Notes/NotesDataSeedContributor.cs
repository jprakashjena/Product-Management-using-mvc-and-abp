using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using NotesModule.Notes;

namespace NotesModule.Notes
{
    public class NotesDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly INoteRepository _noteRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public NotesDataSeedContributor(INoteRepository noteRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _noteRepository = noteRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _noteRepository.InsertAsync(new Note
            (
                id: Guid.Parse("3297aba2-d657-4895-99b9-c9d812ab3057"),
                content: "0",
                identityUserId: null
            ));

            await _noteRepository.InsertAsync(new Note
            (
                id: Guid.Parse("2c97dc25-a964-4f3b-bcdd-05bb3155b73a"),
                content: "3",
                identityUserId: null
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}