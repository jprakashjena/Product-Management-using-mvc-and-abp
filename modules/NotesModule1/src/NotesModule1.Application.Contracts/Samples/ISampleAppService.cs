using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace NotesModule1.Samples;

public interface ISampleAppService : IApplicationService
{
    Task<SampleDto> GetAsync();

    Task<SampleDto> GetAuthorizedAsync();
}
