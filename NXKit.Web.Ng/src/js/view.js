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

    };
}]);