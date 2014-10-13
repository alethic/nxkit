NXKit.define(['require', 'jquery'], function (require, $) {
    return {
        load: function (name, parentRequire, onload, config) {

            // obtain search paths
            var paths = (config['nxkit'] || {})['paths'] || [];
            if (typeof paths === 'string') {
                paths = [paths];
            } else if (paths.length == 0) {
                paths.push('');
            }

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
                var func = function (index) {
                    var url = paths[index];

                    // are we out of paths?
                    if (url == null) {
                        var msg = 'no paths available to search';
                        onload.error(require.makeError('nx-html', msg, new Error(msg), [name]));
                        return;
                    }

                    // append name
                    url = url + name;

                    // generate and load new element
                    div = $(document.createElement('div'))
                        .attr('data-nx-html', name)
                        .load(url, function (response, status) {
                            if (status === 'success' || status === 'notmodified') {

                                // append new element to host container
                                $(div).appendTo($(host));

                                // notify of load
                                onload(div);
                            } else {
                                // try next path
                                func(index + 1);
                            }
                        })
                        .get(0);
                };

                func(0);
            } else {
                onload(div);
            }
        },
    }
});
