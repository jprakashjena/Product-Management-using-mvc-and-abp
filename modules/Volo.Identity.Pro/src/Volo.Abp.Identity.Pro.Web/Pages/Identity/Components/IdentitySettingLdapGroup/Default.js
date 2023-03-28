(function ($) {

    $(function () {
        var l = abp.localization.getResource('AbpIdentity');

        $("#IdentityLdapSettingsForm").on("submit", function (event) {
            event.preventDefault();
            var form = $(this).serializeFormToObject();

            volo.abp.identity.identitySettings.updateLdap(form).then(function (result) {
                $(document).trigger("AbpSettingSaved");
            });
        });
    });

})(jQuery);
