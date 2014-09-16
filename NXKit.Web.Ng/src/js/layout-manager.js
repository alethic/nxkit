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
