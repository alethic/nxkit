/// <reference path="../Util.ts" />
var NXKit;
(function (NXKit) {
    (function (Web) {
        (function (Knockout) {
            var TemplateBindingHandler = (function () {
                function TemplateBindingHandler() {
                }
                TemplateBindingHandler.prototype.init = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                    return ko.bindingHandlers.template.init(element, TemplateBindingHandler.ConvertValueAccessor(valueAccessor, viewModel, bindingContext), allBindingsAccessor, viewModel, bindingContext);
                };

                TemplateBindingHandler.prototype.update = function (element, valueAccessor, allBindingsAccessor, viewModel, bindingContext) {
                    return ko.bindingHandlers.template.update(element, TemplateBindingHandler.ConvertValueAccessor(valueAccessor, viewModel, bindingContext), allBindingsAccessor, viewModel, bindingContext);
                };

                /**
                * Converts the given value accessor into a value accessor compatible with the default template implementation.
                */
                TemplateBindingHandler.ConvertValueAccessor = function (valueAccessor, viewModel, bindingContext) {
                    var _this = this;
                    return function () {
                        return Web.Log.Group('TemplateBindingHandler.ConvertValueAccessor', function () {
                            Web.Log.Object({
                                value: valueAccessor(),
                                viewModel: viewModel
                            });

                            // resolve the view model to be passed to the template
                            var data = _this.GetTemplateViewModel(valueAccessor, viewModel, bindingContext);
                            if (data == null || Object.getOwnPropertyNames(data).length == 0) {
                                throw new Error('unknown viewModel');
                            }

                            // resolve the options to use to look up the template
                            var opts = _this.GetTemplateOptions(valueAccessor, viewModel, bindingContext);
                            if (opts == null || Object.getOwnPropertyNames(opts).length == 0) {
                                throw new Error('unknown template options');
                            }

                            // resolve the template name from the options
                            var name = _this.GetTemplateName(bindingContext, opts);
                            if (name == null) {
                                throw new Error('unknown template');
                            }

                            Web.Log.Object({
                                data: data,
                                opts: opts,
                                name: name
                            });

                            return {
                                data: data,
                                name: name
                            };
                        });
                    };
                };

                /**
                * Gets the recommended view model for the given binding information.
                */
                TemplateBindingHandler.GetTemplateViewModel = function (valueAccessor, viewModel, bindingContext) {
                    var value = valueAccessor();

                    // value itself is a node
                    if (value != null && ko.unwrap(value) instanceof Web.Node)
                        return value;

                    // specified data value
                    if (value != null && value.data != null)
                        return value.data;

                    // specified node value
                    if (value != null && value.node != null && ko.unwrap(value.node) instanceof Web.Node)
                        return value.node;

                    // default to existing view model
                    return viewModel;
                };

                /**
                * Extracts template index data from the given binding information.
                */
                TemplateBindingHandler.GetTemplateOptions = function (valueAccessor, viewModel, bindingContext) {
                    return NXKit.Web.Util.GetLayoutManager(bindingContext).GetTemplateOptions_(valueAccessor, viewModel, bindingContext, {});
                };

                /**
                * Determines the named template from the given extracted data and context.
                */
                TemplateBindingHandler.GetTemplateName = function (bindingContext, data) {
                    return NXKit.Web.Util.GetLayoutManager(bindingContext).GetTemplateName(data);
                };
                return TemplateBindingHandler;
            })();
            Knockout.TemplateBindingHandler = TemplateBindingHandler;

            ko.bindingHandlers['nxkit_template'] = new TemplateBindingHandler();
            ko.virtualElements.allowedBindings['nxkit_template'] = true;
        })(Web.Knockout || (Web.Knockout = {}));
        var Knockout = Web.Knockout;
    })(NXKit.Web || (NXKit.Web = {}));
    var Web = NXKit.Web;
})(NXKit || (NXKit = {}));
//# sourceMappingURL=TemplateBindingHandler.js.map
