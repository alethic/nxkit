NXKit.define(['require', 'jquery'], function (require, $) {
    return {
        load: function (name, parentRequire, onload, config) {
            // find or create host container
            var host = $('body>*[data-nx-html-host]').get(0);
            if (host == null)
                host = $(document.createElement('div'))
                    .attr('data-nx-html-host', '')
                    .css('display', 'none')
                    .prependTo('body')
                    .get(0);

            // find or begin load
            var div = $(host).children("[data-nx-html='" + name + "']").get(0);
            if (div == null) {
                var url = require.toUrl(name);
                if (url) {
                    div = $(document.createElement('div'))
                        .attr('data-nx-html', name)
                        .load(url, function (response, status) {
                            if (status === 'success' || status === 'notmodified') {

                                // append new element to host container
                                $(div).appendTo($(host));

                                // notify of load
                                onload(div);
                            } else {
                                // notify of error
                                onload.error(require.makeError('nx-html', status, new Error(status), [name]));
                            }
                        })
                        .get(0);
                }
            } else {
                onload(div);
            }
        },
    }
});
