require.config({
    baseUrl: 'Scripts',
    paths: {
        'jquery': 'jquery-2.1.1',
        'knockout': 'knockout-3.2.0',
        'semantic': '../Content/semantic/packaged/javascript/semantic',
        'nx-js': ['/NXKit.axd/?m=nx-js'],
        'nx-html': ['/NXKit.axd/?m=nx-html'],
        'nx-css': ['/NXKit.axd/?m=nx-css']
    },
    shim: {
        'semantic': {
            exports: 'semantic',
            deps: ['jquery'],
        },
        'angular': {
            exports: 'angular',
        },
    },
    nxkit: {
        paths: ['/NXKit.axd/?m='],
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