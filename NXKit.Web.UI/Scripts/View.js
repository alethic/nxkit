Type.registerNamespace('_NXKit.Web.UI');

_NXKit.Web.UI.View = function (element) {
    var self = this;
    _NXKit.Web.UI.View.initializeBase(self, [element]);

    self._view = null;
    self._sendFunc = null;
};

_NXKit.Web.UI.View.prototype = {

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

        // update the hidden data field value before submit
        if (self._view != null) {
            $(data).val(JSON.stringify(self._view.Data));
        }
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

        $(document).ready(function () {
            // generate new view
            if (self._view == null) {
                self._view = new NXKit.Web.View(body[0], function (data, cb) {
                    self.send(data, cb);
                });
            }

            // update view with data, and remove data from resubmission
            self._view.Receive(JSON.parse($(data).val()));
            $(data).val('');
        });
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
