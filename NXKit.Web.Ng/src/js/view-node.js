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
        template:
'           <div ng-if="node">' +
'               <div ng-repeat="node in node.Nodes">' +
'                   <div ng-view-node node="node" push="push()" template-provider="templateProvider()"></div>' +
'               </div>' +
'           </div>',
        link: function (scope, element, attrs) {
            if (ng.isArray(scope.node.Nodes)) {
                element.
            }
        },
    };
});

module.controller('nxViewNode', ['$scope', '$attrs', function ($scope, $attrs) {
    this.init = function (element) {

    };
}]);