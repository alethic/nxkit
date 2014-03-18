/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="View.ts" />
var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (XForms) {
            var VisualViewModel = (function (_super) {
                __extends(VisualViewModel, _super);
                function VisualViewModel(context, visual) {
                    _super.call(this, context, visual);
                    var self = this;
                    self._context = context;
                    self._visual = visual;
                }
                return VisualViewModel;
            })(NXKit.Web.VisualViewModel);
            XForms.VisualViewModel = VisualViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
//# sourceMappingURL=XForms.js.map
