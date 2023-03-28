(function ($) {

    $(function () {
        var l = abp.localization.getResource('AbpIdentity');

        $("#IdentityOAuthSettingsForm").on("submit", function (event) {
            event.preventDefault();
            var form = $(this).serializeFormToObject();

            volo.abp.identity.identitySettings.updateOAuth(form).then(function (result) {
                $(document).trigger("AbpSettingSaved");
            });
        });
    });

})(jQuery);
