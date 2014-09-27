Type.registerNamespace('_NXKit.Web.UI');

NXKit = NXKit || {};
NXKit.module = NXKit.module || {};

NXKit.define = function (name, deps, cb) {

};

NXKit.require = function (deps, cb) {

};

_NXKit.Web.UI.View = function (element) {
    var self = this;
    _NXKit.Web.UI.View.initializeBase(self, [element]);

    self._view = null;
    self._sendFunc = null;
    self._enableScriptManager = true;
    self._enableAMD = false;
};

_NXKit.Web.UI.View.prototype = {

    require: function (deps, cb) {
        var self = this;

        if (self._enableAMD) {
            if (typeof require === 'function' && define['amd']) {
                require(deps, cb);
            }
        } else {
            // build resulting list
            var defs = [];

            // build dependency output list
            for (var i = 0; i < deps.length; i++) {

                // extract module parts
                var p = deps[i].indexOf('!');
                var type = p === -1 ? 'js' : deps[i].substring(0, p);
                var name = p === -1 ? deps[i] : deps[i].substring(p + 1);

                var deferred = _NXKit.Web.UI.defines[name];
                if (deferred == null) {
                    deferred = _NXKit.Web.UI.defines[name] = $.Deferred();

                    // url to retrieve module
                    var url = self._handlerUrl + '?m=' + name;

                    if (type === 'js') {
                        $.getScript(url, function (data, status, jqxhr) {
                            deferred.resolve(true);
                        });
                    } else if (type === 'css') {
                        var link = $(document.createElement('link'))
                            .attr('data-nx-require', name)
                            .attr('rel', 'stylesheet')
                            .attr('type', 'text/css')
                            .attr('href', url)
                            .appendTo($('head'))
                            .bind('load', function () {
                                deferred.resolve(link.get(0));
                            });
                    } else if (type === 'nx-template') {
                        var div1 = $('body>*[data-nx-template-host]');
                        if (div1.length == 0)
                            div1 = $(document.createElement('div'))
                                .attr('data-nx-template-host', '')
                                .css('display', 'none')
                                .prependTo('body');
                        var div2 = $(document.createElement('div'))
                            .attr('data-nx-require', name)
                            .appendTo(div1)
                            .load(url, function () {
                                deferred.resolve(div2.get(0));
                            });
                    } else {
                        throw new Error('Unknown module type.');
                    }
                }

                // add to output list
                defs[i] = deferred;
            }

            // invoke call back when all dependencies are resolved
            $.when.apply($, defs)
                .always(function () {
                    console.log(deps);
                })
                .always(cb);
        }

    },

    initialize: function () {
        var self = this;
        _NXKit.Web.UI.View.callBaseMethod(self, 'initialize');

        self._init();
    },

    dispose: function () {
        var self = this;
        _NXKit.Web.UI.View.callBaseMethod(self, 'dispose');

        self._view = null;
        self._send = null;
    },

    get_enableScriptManager: function () {
        return this._enableScriptManager;
    },

    set_enableScriptManager: function (value) {
        this._enableScriptManager = value;
    },

    get_enableAMD: function () {
        return this._enableAMD;
    },

    set_enableAMD: function (value) {
        this._enableAMD = value;
    },

    get_sendFunc: function () {
        return this._sendFunc;
    },

    set_sendFunc: function (value) {
        this._sendFunc = value;
    },

    get_handlerUrl: function () {
        return this._handlerUrl;
    },

    set_handlerUrl: function (value) {
        this._handlerUrl = value;
    },

    _onsubmit: function () {
        var self = this;

        var data = $(self.get_element()).find('>.data');
        if (data.length == 0)
            throw new Error("cannot find data element");

        self.require(function (nx) {
            // update the hidden data field value before submit
            if (self._view != null) {
                $(data).val(JSON.stringify(self._view.Data));
            }
        });
    },

    _init: function () {
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
            self._onsubmit();
        });

        self.require(['nxkit'], function (nx) {

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

        self._sendEval(data, cb);
    },

    _sendEval: function (args, cb) {
        this._sendEvalExec(JSON.stringify(args), function (_) { cb(JSON.parse(_)); });
    },

    _sendEvalExec: function (args, cb) {
        eval(this._sendFunc);
    },

};

_NXKit.Web.UI.View.registerClass('_NXKit.Web.UI.View', Sys.UI.Control);
