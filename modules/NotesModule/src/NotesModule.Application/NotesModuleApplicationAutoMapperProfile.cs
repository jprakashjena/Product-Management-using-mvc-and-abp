using Volo.Abp.Identity;
using System;
using NotesModule.Shared;
using Volo.Abp.AutoMapper;
using NotesModule.Notes;
using AutoMapper;

namespace NotesModule;

public class NotesModuleApplicationAutoMapperProfile : Profile
{
    public NotesModuleApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */

        CreateMap<Note, NoteDto>();
        CreateMap<Note, NoteExcelDto>();

        CreateMap<NoteWithNavigationProperties, NoteWithNavigationPropertiesDto>();
        CreateMap<IdentityUser, LookupDto<Guid>>().ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.UserName));
    }
}