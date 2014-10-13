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

            // generate search list
            var search = [];
            for (var i = 0; i < paths.length; i++) {
                search.push(paths[i] + name);
            }

            // map module name to search path
            var conf = {};
            conf[name] = search;

            // inject configuration
            require.config({
                paths: conf
            })

            var func = function (index) {
                var url = paths[index];

                // are we out of paths?
                if (url == null) {
                    var msg = 'no paths available to search';
                    onload.error(require.makeError('nx-js', msg, new Error(msg), [name]));
                    return;
                }

                // append name
                url = url + name;

                // attempt to retrieve url
                $.get(url, function (response, status) {
                    if (status === 'success' || status === 'notmodified') {
                        onload.fromText(response);
                    } else {
                        // try next path
                        func(index + 1);
                    }
                });
            };

            func(0);
        },

    }
});
