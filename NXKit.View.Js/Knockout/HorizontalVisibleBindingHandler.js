$.fn.extend({
    slideRightShow: function () {
        return this.each(function () {
            $(this).show('slide', { direction: 'right' }, 1000);
        });
    },
    slideLeftHide: function () {
        return this.each(function () {
            $(this).hide('slide', { direction: 'left' }, 1000);
        });
    },
    slideRightHide: function () {
        return this.each(function () {
            $(this).hide('slide', { direction: 'right' }, 1000);
        });
    },
    slideLeftShow: function () {
        return this.each(function () {
            $(this).show('slide', { direction: 'left' }, 1000);
        });
    }
});

var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var HorizontalVisibleBindingHandler = (function () {
                function HorizontalVisibleBindingHandler() {
                }
                HorizontalVisibleBindingHandler.prototype.init = function (element, valueAccessor) {
                    var value = valueAccessor();
                    $(element).toggle(ko.utils.unwrapObservable(value));
                    ko.utils.unwrapObservable(value) ? $(element)['slideLeftShow']() : $(element)['slideLeftHide']();
                };

                HorizontalVisibleBindingHandler.prototype.update = function (element, valueAccessor) {
                    var value = valueAccessor();
                    ko.utils.unwrapObservable(value) ? $(element)['slideLeftShow']() : $(element)['slideLeftHide']();
                };
                return HorizontalVisibleBindingHandler;
            })();
            Knockout.HorizontalVisibleBindingHandler = HorizontalVisibleBindingHandler;

            ko.bindingHandlers['nxkit_hvisible'] = new HorizontalVisibleBindingHandler();
            ko.virtualElements.allowedBindings['nxkit_hvisible'] = true;
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
//# sourceMappingURL=HorizontalVisibleBindingHandler.js.map
