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

            var link = $('head').children("[data-nx-css='" + name + "']").get(0);
            if (link == null) {
                var func = function (index) {
                    var url = paths[index];

                    // are we out of paths?
                    if (url == null) {
                        var msg = 'no paths available to search';
                        onload.error(require.makeError('nx-css', msg, new Error(msg), [name]));
                        return;
                    }

                    // append name
                    url = url + name;

                    // attempt to retrieve url
                    $.get(url, function (response, status) {
                        if (status === 'success' || status === 'notmodified') {
                            // on success add element
                            onload(link = $(document.createElement('link'))
                                .attr('data-nx-css', name)
                                .attr('rel', 'stylesheet')
                                .attr('type', 'text/css')
                                .attr('href', url)
                                .prependTo($('head'))
                                .get(0));
                        } else {
                            // try next path
                            func(index + 1);
                        }
                    });
                };

                func(0);
            } else {
                onload(link);
            }
        },
    }
});
