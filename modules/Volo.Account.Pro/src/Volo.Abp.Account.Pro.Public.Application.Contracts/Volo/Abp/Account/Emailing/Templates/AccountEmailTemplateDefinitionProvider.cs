﻿using Volo.Abp.Account.Localization;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Localization;
using Volo.Abp.TextTemplating;

namespace Volo.Abp.Account.Emailing.Templates;

public class AccountEmailTemplateDefinitionProvider : TemplateDefinitionProvider
{
    public override void Define(ITemplateDefinitionContext context)
    {
        context.Add(
            new TemplateDefinition(
                AccountEmailTemplates.PasswordResetLink,
                displayName: LocalizableString.Create<AccountResource>($"TextTemplate:{AccountEmailTemplates.PasswordResetLink}"),
                layout: StandardEmailTemplates.Layout,
                localizationResource: typeof(AccountResource)
            ).WithVirtualFilePath("/Volo/Abp/Account/Emailing/Templates/PasswordResetLink.tpl", true)
        );

        context.Add(
            new TemplateDefinition(
                AccountEmailTemplates.EmailConfirmationLink,
                displayName: LocalizableString.Create<AccountResource>($"TextTemplate:{AccountEmailTemplates.EmailConfirmationLink}"),
                layout: StandardEmailTemplates.Layout,
                localizationResource: typeof(AccountResource)
            ).WithVirtualFilePath("/Volo/Abp/Account/Emailing/Templates/EmailConfirmationLink.tpl", true)
        );

        context.Add(
            new TemplateDefinition(
                AccountEmailTemplates.EmailSecurityCode,
                displayName: LocalizableString.Create<AccountResource>($"TextTemplate:{AccountEmailTemplates.EmailSecurityCode}"),
                layout: StandardEmailTemplates.Layout,
                localizationResource: typeof(AccountResource)
            ).WithVirtualFilePath("/Volo/Abp/Account/Emailing/Templates/EmailSecurityCode.tpl", true)
        );
    }
}
