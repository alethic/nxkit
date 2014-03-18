/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
/// <reference path="Scripts/typings/knockout/knockout.d.ts" />
/// <reference path="View.ts" />
/// <reference path="XForms.ts" />
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
            var GroupViewModel = (function (_super) {
                __extends(GroupViewModel, _super);
                function GroupViewModel(context, visual) {
                    _super.call(this, context, visual);
                    var self = this;
                }
                Object.defineProperty(GroupViewModel.prototype, "Label", {
                    get: function () {
                        return NXKit.Web.XForms.VisualViewModel.GetLabel(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(GroupViewModel.prototype, "Help", {
                    get: function () {
                        return NXKit.Web.XForms.VisualViewModel.GetHelp(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(GroupViewModel.prototype, "Contents", {
                    get: function () {
                        return NXKit.Web.XForms.VisualViewModel.GetRenderableContents(this.Visual);
                    },
                    enumerable: true,
                    configurable: true
                });
                return GroupViewModel;
            })(NXKit.Web.XForms.VisualViewModel);
            XForms.GroupViewModel = GroupViewModel;
        })(Web.XForms || (Web.XForms = {}));
        var XForms = Web.XForms;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
//# sourceMappingURL=XFormsGroup.js.map
