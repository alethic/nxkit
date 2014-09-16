define([
    'jquery',
    'angular',
    'nxkit',
    'knockout',
], function ($, ng, nx, ko) {
var module = angular.module('nx', []);
module.service('nxLayoutManager', function ($q, $document) {
    
    var document = $($document[0]);
    document.append(
        '<script type="text/html"' +
            'id="NXKit.View">' +
            '' +
            '<!-- ko foreach: $data.Messages -->' +
            '<div class="ui message" data-bind="' +
                'css: {' +
                'green: Severity === 1,' +
                'blue: Severity === 2,' +
                'yellow: Severity ===3,' +
                'red: Severity === 4,' +
                '}">' +
                '<i class="close icon" data-bind="click: function () { $parent.RemoveMessage($data) }"></i>' +
                '<div class="header">' +
                    '<span data-bind="text: Text" />' +
                '</div>' +
            '</div>' +
            '<!-- /ko -->' +
            '<!-- ko nxkit_layout_manager_export -->' +
            '<!-- ko nxkit_template: $data.Root -->' +
            '<!-- /ko -->' +
            '<!-- /ko -->' +
        '</script>' +
        '');
    
    var __extends = this.__extends || function (d, b) {
        for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
        function __() { this.constructor = d; }
        __.prototype = b.prototype;
        d.prototype = new __();
    };
    
    var NgLayoutManager = (function (_super) {
        __extends(NgLayoutManager, _super);
        
        function NgLayoutManager(context) {
            _super.call(this, context);
        }
        
        NgLayoutManager.prototype.GetLocalTemplates = function () {
            return $('script[type="text/html"]').toArray();
        };
        return NgLayoutManager;
    })(nx.Web.DefaultLayoutManager);
    
    nx.Web.ViewModelUtil.LayoutManagers.unshift(function (c) {
        return new NgLayoutManager(c);
    });

});

module.directive('nxView', ['nxLayoutManager', function ($compile) {
    return {
        restrict: 'E',
        scope: {
            url: '@url',
        },
        controller: 'nxView',
        template:
'           <div class="host" data-bind="template { name: \'d\' }">' +
'               <div class="body"></div>' +
'           </div>',
        link: function ($scope, element, attrs, ctrl) {
            ctrl.init(element, $scope.url);
        },
    };
}]);

module.controller('nxView', ['$scope', '$attrs', '$http', function ($scope, $attrs, $http) {
    
    this.init = function (element, url) {
        $scope.url = url;
        
        // locate element for view body
        var host = $(element[0]).find('>.host');
        if (host.length == 0)
            throw new Error("cannot find host element");
        
        // locate element for view body
        var body = $(host).find('>.body');
        if (body.length == 0)
            throw new Error("cannot find body element");
        
        // initialize view
        if ($scope.view == null) {
            $scope.view = new nx.Web.View(body[0], function (data, cb) {
                this.send(data, cb);
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