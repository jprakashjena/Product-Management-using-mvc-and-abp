using NotesModule.Web.Pages.NotesModule.Notes;
using Volo.Abp.AutoMapper;
using NotesModule.Notes;
using AutoMapper;

namespace NotesModule.Web;

public class NotesModuleWebAutoMapperProfile : Profile
{
    public NotesModuleWebAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<NoteDto, NoteUpdateViewModel>();
        CreateMap<NoteUpdateViewModel, NoteUpdateDto>();
        CreateMap<NoteCreateViewModel, NoteCreateDto>();
    }
}