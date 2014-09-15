define([
    'angular',
    './_module',
], function (ng, app) {
    app.controller('index', function ($scope, $http) {

        $scope.data = {};

        $scope.init = function () {
            $http.get('/api/view/form')
                .success(function (response) {
                    $scope.data = response;
                })
                .error(function (response) {
                    console.log(response);
                });
        }

        $scope.push = function (data) {
            $http.post('/api/view/form', data)
                .success(function (response) {
                    $scope.data = response;
                })
                .error(function (response) {
                    console.log(response);
                });
        };

        $scope.getTemplate = function (data) {
            return null;
        };

        $scope.init();

    });
});