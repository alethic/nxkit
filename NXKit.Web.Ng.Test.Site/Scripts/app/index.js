define([
    'angular',
    './_module',
], function (ng, app) {
    app.controller('index', function ($scope) {
        $scope.data = {
            'item1': 'value1',
            'item2': 'value2',
            'item3': 'value3',
        };
        $scope.push = function (data) {
            console.log('push: ' + data);
        };
        $scope.getTemplate = function (data) {
            console.log('getTemplate: ' + data);
        };
    });
});