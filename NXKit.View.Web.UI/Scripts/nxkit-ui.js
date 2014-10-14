Type.registerNamespace('_NXKit.View.Web.UI');

_NXKit.View.Web.UI.View = function (element) {
    var self = this;
    _NXKit.View.Web.UI.View.initializeBase(self, [element]);
};

_NXKit.View.Web.UI.View.prototype = {

    initialize: function () {
        var self = this;
        _NXKit.View.Web.UI.View.callBaseMethod(self, 'initialize');

        self.init();
    },

    dispose: function () {
        var self = this;
        _NXKit.View.Web.UI.View.callBaseMethod(self, 'dispose');
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

        NXKit.require([
            'nx-js!nxkit'],
            function (nx) {
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

        // hook into submission life cycle
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(function (s, a) {
            self.onsubmit();
        });

        // load NXKit implementation
        NXKit.require([
                'nx-js!nxkit'],
            function (nx) {

                // initialize view
                if (self._view == null) {
                    self._view = new nx.View.View(body[0], function (data, cb) {
                        self.send({ Type: 'Message', Data: data }, cb);
                    });
                }

                // update view with initial data set
                self._view.Receive(JSON.parse($(data).val()));
                $(data).val('');
            });
    },

    send: function (data, wh) {
        var self = this;

        // initiate server request
        var cb = function (response) {
            if (response.Type === 'Message') {
                wh(response.Message);
            }
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

_NXKit.View.Web.UI.View.registerClass('_NXKit.View.Web.UI.View', Sys.UI.Control);
