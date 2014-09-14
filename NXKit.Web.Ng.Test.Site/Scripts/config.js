require.config({
    baseUrl: 'Scripts',
    paths: {

    },
    shim: {
        'angular':{
            exports: 'angular',
        }
    }
});

require([
    'angular',
    'app/_index'],
    function (ng, app) {
        ng.bootstrap(document, [app.name]);
    });