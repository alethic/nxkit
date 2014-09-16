require.config({
    baseUrl: 'Scripts',
    paths: {
        'jquery': 'jquery-2.1.1',
        'knockout': 'knockout-3.2.0'
    },
    shim: {
        'angular': {
            exports: 'angular',
        },
        'nxkit': {
            deps: ['knockout'],
        }
    }
});

require([
    'angular',
    'app/_index'],
    function (ng, app) {
        ng.bootstrap(document, [app.name]);
    });