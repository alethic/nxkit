NXKit.define('nx-css', ['jquery'], function ($) {
    return {
        load: function (name, parentRequire, onload, config) {
            parentRequire(['jquery'], function ($) {
                if ($('head').children("[data-nx-require='" + name + "']").length == 0) {
                    var link = $(document.createElement('link'))
                        .attr('data-nx-require', name)
                        .attr('rel', 'stylesheet')
                        .attr('type', 'text/css')
                        .attr('href', _NXKit.View.Web.UI.handlerUrl + '?m=' + name)
                        .appendTo($('head'))
                        .bind('load', function () {
                            onload(link.get(0));
                        });
                }
            });
        },
    }
});