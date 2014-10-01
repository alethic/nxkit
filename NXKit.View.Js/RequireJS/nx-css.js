NXKit.define(['require', 'jquery'], function (require, $) {
    return {
        load: function (name, parentRequire, onload, config) {
            var link = $('head').children("[data-nx-require='" + name + "']").get(0);
            if (link == null) {
                link = $(document.createElement('link'))
                    .attr('data-nx-require', name)
                    .attr('rel', 'stylesheet')
                    .attr('type', 'text/css')
                    .attr('href', require.toUrl(name))
                    .appendTo($('head'))
                    .bind('load', function () {
                        onload(link);
                    }).get(0);
            } else {
                onload(link);
            }
        }
    };
});