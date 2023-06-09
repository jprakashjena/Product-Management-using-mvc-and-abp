(function ($) {
    $(function () {

        $("#AccountSettingsForm").on("submit", function (event) {
            event.preventDefault();
            var form = $(this).serializeFormToObject();

            volo.abp.account.accountSettings.update(form).then(function (result) {
                $(document).trigger("AbpSettingSaved");
            });
        });

        $("#AccountTwoFactorSettingsForm").on("submit", function (event) {
            event.preventDefault();
            var form = $(this).serializeFormToObject();

            volo.abp.account.accountSettings
                .updateTwoFactor(form)
                .then(function (result) {
                    $(document).trigger("AbpSettingSaved");
                });
        });

        $("#AccountCaptchaSettingsForm").on('submit', function (event) {
            event.preventDefault();
            var form = $(this).serializeFormToObject();

            volo.abp.account.accountSettings
                .updateRecaptcha(form)
                .then(function (result) {
                    $(document).trigger("AbpSettingSaved");
                });
        });

        $("#AccountTwoFactorSettings_TwoFactorBehaviour").change(function () {
            if (this.value !== "Optional") {
                $("#AccountTwoFactorSettings_UsersCanChange").parent().hide();
            } else {
                $("#AccountTwoFactorSettings_UsersCanChange").parent().show();
            }
        }).change();

        $("#AccountExternalProviderSettingsForm input:checkbox").change(function () {
            if ($(this).prop("checked")) {
                $("#" + $(this).data("collapse") + " input").val("");
                $("#" + $(this).data("collapse")).collapse("hide");
            } else {
                $("#" + $(this).data("collapse")).collapse("show");
            }
        });

        var accountSettingPage = {

            $scoreFormGroup : $('input[name=Score]').parent(),

            $versionSelect : $('select[name=Version]'),

            onCaptchaVersionChange : function(){
                accountSettingPage.$versionSelect.change(()=>{
                    accountSettingPage.showOrHideScoreFormGroup();
                })
            },

            showOrHideScoreFormGroup : function(){
                var version = accountSettingPage.$versionSelect.val();

                if (version === '3') {
                    accountSettingPage.$scoreFormGroup.show();
                } else {
                    accountSettingPage.$scoreFormGroup.hide();
                }
            },

            init : function(){
                accountSettingPage.showOrHideScoreFormGroup();
                accountSettingPage.onCaptchaVersionChange();
            }
        };

        accountSettingPage.init();

    });
})(jQuery);
