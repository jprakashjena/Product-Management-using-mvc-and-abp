﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Users;

namespace Volo.Abp.Account.Public.Web.Pages.Account.Components.ProfileManagementGroup.ProfilePicture;

public class AccountProfilePictureManagementGroupViewComponent : AbpViewComponent
{
    public ICurrentUser CurrentUser { get; }

    protected IAccountAppService AccountAppService;

    public AccountProfilePictureManagementGroupViewComponent(IAccountAppService accountAppService, ICurrentUser currentUser)
    {
        CurrentUser = currentUser;
        AccountAppService = accountAppService;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var profilePictureSource = await AccountAppService.GetProfilePictureAsync(CurrentUser.GetId());

        var uploadProfilePictureInfoModel = new UploadProfilePictureInfoModel
        {
            Type = profilePictureSource.Type
        };

        return View("~/Pages/Account/Components/ProfileManagementGroup/ProfilePicture/Default.cshtml", uploadProfilePictureInfoModel);
    }

    public class UploadProfilePictureInfoModel
    {
        public ProfilePictureType Type { get; set; }

        public IFormFile Picture { get; set; }
    }
}
