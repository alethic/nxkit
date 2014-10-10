require.config({
    baseUrl: 'Scripts',
    paths: {
        'jquery': 'jquery-2.1.1',
        'knockout': 'knockout-3.2.0',
        'semantic': '../Content/semantic/packaged/javascript/semantic',
        'nxkit.html': '../Content/nxkit.html',
        'nxkit.css': '../Content/nxkit.css',
    },
    shim: {
        'semantic': {
            exports: 'semantic',
            deps: ['jquery'],
        },
        'angular': {
            exports: 'angular',
        },
        'nxkit': {
            deps: ['knockout'],
        }
    },
    deps: ['semantic'],
});

require([
    'angular',
    'app/_index'],
    function (ng, app) {
        ng.bootstrap(document, [app.name]);
    }
);