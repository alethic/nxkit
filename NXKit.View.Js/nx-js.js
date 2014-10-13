NXKit.define([], function () {
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
            NXKit.require.config({
                paths: conf
            })

            // dispatch back to require
            NXKit.require([name],
                function (result) {
                    onload(result);
                },
                function (error) {
                    onload.error(error);
                });
        },

    }
});
