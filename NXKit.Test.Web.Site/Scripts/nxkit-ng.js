define([
    'angular',
], function (ng) {
var module = angular.module('nx', []);
module.directive('nxView', function () {
    return {
        template: 'View Goes Here',
    };
});

});