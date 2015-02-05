require.config({
    baseUrl: 'Scripts',
    paths: {
        'jquery': 'jquery-2.1.1',
        'knockout': 'knockout-3.2.0',
        'semantic': '../Content/semantic/packaged/javascript/semantic',
        'nx-js': ['/api/view/module/nx-js'],
        'nx-html': ['/api/view/module/nx-html'],
        'nx-css': ['/api/view/module/nx-css']
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
        paths: ['/api/view/module/'],
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