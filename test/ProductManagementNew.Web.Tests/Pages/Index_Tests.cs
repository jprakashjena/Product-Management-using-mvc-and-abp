using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace ProductManagementNew.Pages;

public class Index_Tests : ProductManagementNewWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
