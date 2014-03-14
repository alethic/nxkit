/// <reference path="Scripts/typings/jquery/jquery.d.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        var View = (function () {
            function View(element) {
                this._element = element;
            }
            Object.defineProperty(View.prototype, "element", {
                get: function () {
                    return this._element;
                },
                set: function (value) {
                    this._element = value;
                },
                enumerable: true,
                configurable: true
            });


            Object.defineProperty(View.prototype, "visual", {
                get: function () {
                    return this._visual;
                },
                set: function (value) {
                    console.log('logged');
                    this._visual = value;
                },
                enumerable: true,
                configurable: true
            });

            return View;
        })();
        Web.View = View;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
//# sourceMappingURL=View.js.map
