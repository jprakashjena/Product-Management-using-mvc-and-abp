(function ($) {
    $(function () {

        $("#AccountExternalProviderSettingsForm input:checkbox").change(function () {
            if ($(this).prop("checked")) {
                $("#" + $(this).data("collapse") + " input").val("");
                $("#" + $(this).data("collapse")).collapse("hide");
            } else {
                $("#" + $(this).data("collapse")).collapse("show");
            }
        });

        $("#AccountExternalProviderSettingsForm").on("submit", function (event) {
            event.preventDefault();
            var form = [];
            $(".provider_container").each(function () {
                var obj = $(this).find("input").serializeFormToObject();
                form.push({
                    "name": obj.name,
                    "enabled": obj.enabled,
                    "properties": Object.keys(obj.properties).map(function (e) {
                        return {"name": e, "value": obj.properties[e]};
                    }),
                    "secretProperties": Object.keys(obj.secretProperties).map(function (e) {
                        return {"name": e, "value": obj.secretProperties[e]};
                    }),
                })
            });

            volo.abp.account.accountSettings
                .updateExternalProvider(form)
                .then(function (result) {
                    $(document).trigger("AbpSettingSaved");
                });
        });

    });
})(jQuery);
