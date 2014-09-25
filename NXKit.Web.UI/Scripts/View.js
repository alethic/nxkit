Type.registerNamespace('_NXKit.Web.UI');

_NXKit.Web.UI.defines = {};

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
            for (var i in deps) {
                if (!Object.prototype.hasOwnProperty.call(_NXKit.Web.UI.defines, deps[i])) {
                    self.send({ Type: 'Require', Require: deps[i] }, function (response) {
                        console.log(response);
                    })
                }
            }
        }

        if (self._enableScriptManager) {
            self.require = function (modules, cb) {
                $(document).ready(function () {
                    cb();
                });
            }
        }

    },

    _wait: function (cb) {
        var self = this;

        if (self._enableScriptManager) {
            if (typeof NXKit === 'object' &&
                typeof NXKit.View === 'object' &&
                typeof NXKit.View.View === 'function') {
                $(document).ready(function () {
                    cb(NXKit);
                });
            } else {
                if (typeof console.warn === 'function') {
                    console.warn('NXKit.Web.UI component delayed waiting ScriptManager initialization of NXKit.');
                }

                setTimeout(function () { self._wait(cb) }, 1000);
            }
        }

        if (self._enableAMD) {
            if (typeof require === 'function' && define['amd']) {
                require(['nxkit'], function (nx) {
                    $(document).ready(function () {
                        cb(nx);
                    })
                });
            } else {
                console.error('EnableAMD specified, but AMD not found.');
            }
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

    _onsubmit: function () {
        var self = this;

        var data = $(self.get_element()).find('>.data');
        if (data.length == 0)
            throw new Error("cannot find data element");

        self._require(function (nx) {
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

        self._wait(function (nx) {

            // initialize view
            if (self._view == null) {
                self._view = new nx.View.View(body[0],
                    function (modules, cb) {
                        self.require(modules, cb);
                    },
                    function (data, cb) {
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
