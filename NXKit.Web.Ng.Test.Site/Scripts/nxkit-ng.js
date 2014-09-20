define([
    'jquery',
    'angular',
    'nxkit',
    'knockout',
], function ($, ng, nx, ko) {
var module = angular.module('nx', []);
module.directive('nxView', ['$compile', function ($compile) {
    return {
        restrict: 'E',
        scope: {
            url: '@url',
        },
        controller: 'nxView',
        template:
'           <div class="nx-view">' +
'               <div class="nx-body"></div>' +
'           </div>',
        link: function ($scope, element, attrs, ctrl) {
            ctrl.init(element, $scope.url);
        },
    };
}]);

module.controller('nxView', ['$scope', '$attrs', '$http', function ($scope, $attrs, $http) {
    
    this.init = function (element, url) {
        var self = this;

        $scope.url = url;
        
        // locate element for view body
        var view = $(element[0]).find('>.nx-view');
        if (view.length == 0)
            throw new Error("cannot find host element");
        
        // locate element for view body
        var body = $(view).find('>.nx-body');
        if (body.length == 0)
            throw new Error("cannot find body element");
        
        $(document).ready(function () {
            
            // generate new view
            if ($scope.view == null) {
                $scope.view = new nx.Web.View(body[0], function (data, cb) {
                    self.send(data, cb);
                });
            }
            
            // get initial data
            $http.get($scope.url)
                .success(function (result) {
                    $scope.view.Receive(result);
                })
                .error(function (result) {
                    console.log(result);
                });
        });
    };
    
    this.send = function (data, cb) {
        $http.post($scope.url, data)
            .success(function (result) {
                cb(result);
            })
            .error(function (result) {
                console.log(result);
            });
    };

}]);

});