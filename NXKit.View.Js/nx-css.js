NXKit.define(['require', 'jquery'], function (require, $) {
    return {
        load: function (name, parentRequire, onload, config) {
            var link = $('head').children("[data-nx-css='" + name + "']").get(0);
            if (link == null) {
                var url = require.toUrl(name);
                if (url) {
                    $.get(url, function (response, status) {
                        if (status === 'success' || status === 'notmodified') {
                            onload(link = $(document.createElement('link'))
                                .attr('data-nx-css', name)
                                .attr('rel', 'stylesheet')
                                .attr('type', 'text/css')
                                .attr('href', url)
                                .prependTo($('head'))
                                .get(0));
                        } else {
                            // notify of error
                            onload.error(require.makeError('nx-css', status, new Error(status), [name]));
                        }
                    });
                }
            } else {
                onload(link);
            }
        },
    }
});
