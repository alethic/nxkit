NXKit.define(['require', 'jquery'], function (require, $) {
    return {
        load: function (name, parentRequire, onload, config) {
            // find or create host container
            var div1 = $('body>*[data-nx-html-host]').get(0);
            if (div1 == null)
                div1 = $(document.createElement('div'))
                    .attr('data-nx-html-host', '')
                    .css('display', 'none')
                    .prependTo('body')
                    .get(0);

            // find or begin load
            var div2 = $(div1).children("[data-nx-require='" + name + "']").get(0);
            if (div2 == null) {
                var url = require.toUrl(name);
                if (url) {
                    div2 = $(document.createElement('div'))
                        .attr('data-nx-require', name)
                        .load(url, function (response, status) {
                            if (status === 'success' || status === 'notmodified') {

                                // append new element to host container
                                $(div2).appendTo($(div1));

                                // notify of load
                                onload(div2);
                            } else {
                                // notify of error
                                onload.error(status);
                            }
                        })
                        .get(0);
                }
            }
        },
    }
});
