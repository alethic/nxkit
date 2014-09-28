Type.registerNamespace('_NXKit.Web.UI');

(function () {
    var requirejs = NXKit.requirejs;
    var require = NXKit.require;
    var define = NXKit.define;

    var onError = requirejs.onError;
    requirejs.onError = function (err) {
        if (err.requireModules) {
            var modules = [];
            for (var i in err.requireModules) {
                var failedId = err.requireModules[i];
                if (failedId != null) {
                    var url = _NXKit.Web.UI.handlerUrl + '?m=' + failedId;

                    // check for whether we've already failed here
                    var paths = requirejs.s.contexts._.config.paths || {};
                    var mpath = paths[failedId] || (paths[failedId] = []);
                    if (mpath.indexOf(url) > -1)
                        continue;

                    // add new fall back url
                    requirejs.undef(failedId);
                    mpath.push(url);

                    // redirect to NXKit service
                    requirejs.config({
                        paths: paths,
                    });

                    // retry
                    modules.push(failedId);
                }
            }

            if (modules.length > 0) {
                require(modules, function () {
                    console.log('fixed');
                });
                return;
            }
        }

        console.error(err);

        // send to original function
        onError(err);
    };

    NXKit.define('jquery', [], function () { return $; });
    NXKit.define('knockout', [], function () { return ko; });

    NXKit.define('nx-template', [], function () {
        return {

            load: function (name, parentRequire, onload, config) {
                parentRequire(['jquery'], function ($) {
                    var div1 = $('body>*[data-nx-template-host]');
                    if (div1.length == 0)
                        div1 = $(document.createElement('div'))
                            .attr('data-nx-template-host', '')
                            .css('display', 'none')
                            .prependTo('body');
                    if (div1.children("[data-nx-require='" + name + "']").length == 0) {
                        var div2 = $(document.createElement('div'))
                            .attr('data-nx-require', name)
                            .appendTo(div1)
                            .load(_NXKit.Web.UI.handlerUrl + '?m=' + name, function () {
                                onload(div2.get(0));
                            });
                    }
                });
            },

        }
    });

    NXKit.define('css', [], function () {
        return {

            load: function (name, parentRequire, onload, config) {
                parentRequire(['jquery'], function ($) {
                    if ($('head').children("[data-nx-require='" + name + "']").length == 0) {
                        var link = $(document.createElement('link'))
                            .attr('data-nx-require', name)
                            .attr('rel', 'stylesheet')
                            .attr('type', 'text/css')
                            .attr('href', _NXKit.Web.UI.handlerUrl + '?m=' + name)
                            .appendTo($('head'))
                            .bind('load', function () {
                                onload(link.get(0));
                            });
                    }
                });
            },

        }
    });
})();

_NXKit.Web.UI.View = function (element) {
    var self = this;
    _NXKit.Web.UI.View.initializeBase(self, [element]);
};

_NXKit.Web.UI.View.prototype = {

    initialize: function () {
        var self = this;
        _NXKit.Web.UI.View.callBaseMethod(self, 'initialize');

        self.init();
    },

    dispose: function () {
        var self = this;
        _NXKit.Web.UI.View.callBaseMethod(self, 'dispose');
    },

    get_sendFunc: function () {
        return this._sendFunc;
    },

    set_sendFunc: function (value) {
        this._sendFunc = value;
    },

    onsubmit: function () {
        var self = this;

        var data = $(self.get_element()).find('>.data');
        if (data.length == 0)
            throw new Error("cannot find data element");

        NXKit.require(['nxkit'], function (nx) {
            // update the hidden data field value before submit
            if (self._view != null) {
                $(data).val(JSON.stringify(self._view.Data));
            }
        });
    },

    init: function () {
        var self = this;

        var form = $(self.get_element()).closest('form');
        if (form.length == 0)
            throw new Error('cannot find form element');

        var data = $(self.get_element()).find('>.data');
        if (data.length == 0)
            throw new Error("cannot find data element");

        var body = $(self.get_element()).find('>.body');
        if (body.length == 0)
            throw new Error("cannot find body element");

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(function (s, a) {
            self.onsubmit();
        });

        NXKit.require(['nxkit'], function (nx) {

            // initialize view
            if (self._view == null) {
                self._view = new nx.View.View(body[0], function (deps, cb) { self.require(deps, cb); }, function (data, cb) {
                    self.send({ Type: 'Message', Data: data }, cb);
                });
            }

            // update view with initial data set
            self._view.Receive(JSON.parse($(data).val()));
            $(data).val('');
        })
    },

    send: function (data, wh) {
        var self = this;

        // initiate server request
        var cb = function (response) {
            wh(response);
        };

        self.sendEval(data, cb);
    },

    sendEval: function (args, cb) {
        this.sendEvalExec(JSON.stringify(args), function (_) { cb(JSON.parse(_)); });
    },

    sendEvalExec: function (args, cb) {
        eval(this._sendFunc);
    },

};

_NXKit.Web.UI.View.registerClass('_NXKit.Web.UI.View', Sys.UI.Control);
