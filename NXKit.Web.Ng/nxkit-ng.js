define([
    'angular',
], function (ng) {
var module = angular.module('nx', []);
module.service('nxTemplates', function () {

});

module.directive('nxViewNode', function () {
    return {
        restrict: 'E',
        scope: {
            node: '=node',
            push: '&push',
            templateProvider: '&templateProvider',
        },
        controller: 'nxViewNode',
        link: function (scope, element, attrs, ctrl) {
            ctrl.init(element);
        },
        template: 'Here Node Lies',
    };
});

module.controller('nxViewNode', ['$scope', '$attrs', function ($scope, $attrs) {
    this.init = function (element) {
        console.log($scope.data);
        console.log($scope.push);
        console.log($scope.templateProvider);
    };
}]);
module.directive('nxView', function ($compile) {
    return {
        restrict: 'E',
        scope: {
            data: '=data',
            push: '&push',
            templateProvider: '&templateProvider',
        },
        controller: 'nxView',
        link: function (scope, element, attrs, ctrl) {
            ctrl.init(element);
        },
        template: 
'           <div ng-if="data.Node">' +
'               <div ng-view-node node="data.Node" push="push()" template-provider="templateProvider()"></div>' +
'           </div>'
    };
});

module.controller('nxView', ['$scope', '$attrs', function ($scope, $attrs) {
    this.init = function (element) {
        console.log($scope.data);
        console.log($scope.push);
        console.log($scope.templateProvider);
    };
}]);

});