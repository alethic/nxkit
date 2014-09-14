define([
    'angular',
], function (ng) {
var module = angular.module('nx', []);
module.service('nxTemplates', function () {

});

module.directive('nxView', function () {
    return {
        restrict: 'AE',
        scope: {
            data: '=data',
            push: '&push',
            templateProvider: '&templateProvider',
        },
        controller: 'nxView',
        link: function (scope, element, attrs, ctrl) {
            ctrl.init(element);
        },
        template: '<div nx-view-node></div>',
    };
});

module.service('nxView', ['$http', function ($http) {
    this.foo = {};
}]);

module.controller('nxView', ['$scope', '$attrs', 'nxView', function ($scope, $attrs, nxView) {
    this.init = function (element) {
        console.log($scope.data);
        console.log($scope.push);
        console.log($scope.templateProvider);
        console.log(nxView.foo);
    };
}]);

});