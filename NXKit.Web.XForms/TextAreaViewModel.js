var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports"], function(require, exports) {
    (function (NXKit) {
        (function (Web) {
            (function (XForms) {
                var TextAreaViewModel = (function (_super) {
                    __extends(TextAreaViewModel, _super);
                    function TextAreaViewModel(context, node) {
                        _super.call(this, context, node);
                    }
                    return TextAreaViewModel;
                })(NXKit.Web.XForms.XFormsNodeViewModel);
                XForms.TextAreaViewModel = TextAreaViewModel;
            })(Web.XForms || (Web.XForms = {}));
            var XForms = Web.XForms;
        })(NXKit.Web || (NXKit.Web = {}));
        var Web = NXKit.Web;
    })(exports.NXKit || (exports.NXKit = {}));
    var NXKit = exports.NXKit;
});
