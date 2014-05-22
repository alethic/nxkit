Type.registerNamespace('_NXKit.Web.UI');

_NXKit.Web.UI.View = function (element) {
    var self = this;
    _NXKit.Web.UI.View.initializeBase(self, [element]);

    self._view = null;
    self._data = null;
    self._save = null;
    self._hash = null;
    self._body = null;
    self._push = null;
};

_NXKit.Web.UI.View.prototype = {

    initialize: function () {
        var self = this;
        _NXKit.Web.UI.View.callBaseMethod(self, 'initialize');
    },

    dispose: function () {
        var self = this;
        _NXKit.Web.UI.View.callBaseMethod(self, 'dispose');
    },

    get_view: function () {
        return this._view;
    },

    get_data: function () {
        return this._data;
    },

    set_data: function (value) {
        this._data = value;
        this._init();
    },

    get_save: function () {
        return this._save;
    },

    set_save: function (value) {
        this._save = value;
        this._init();
    },

    get_hash: function () {
        return this._hash;
    },

    set_hash: function (value) {
        this._hash = value;
    },

    get_body: function () {
        return this._body;
    },

    set_body: function (value) {
        this._body = value;
        this._init();
    },

    get_push: function () {
        return this._push;
    },

    set_push: function (value) {
        this._push = value;
    },

    _init: function () {
        var self = this;

        if (self._body != null &&
            self._data != null &&
            self._save != null) {

            // generate new view
            if (self._view == null) {
                self._view = new NXKit.Web.View(self._body, function (commands, cb) {
                    self.sendCommands(commands, cb);
                });
            }

            // update view with data, and remove data from resubmission
            self._view.Receive(JSON.parse($(self._data).val()));
            $(self._data).val('');
        }
    },

    sendCommands: function (commands, wh) {
        var self = this;

        // initiate server request
        var cb = function (response) {
            if (response.Code == 200) {

                // store new save data if available
                if (response.Save != null)
                    $(self._save).val(response.Save);

                // store new hash data if available
                if (response.Hash != null)
                    $(self._hash).val(response.Hash);

                // send results to caller
                wh(response.Data);
            } else if (response.Code == 500) {
                // resend with save data
                self.sendCommandsEval({
                    Save: $(self._save).val(),
                    Hash: $(self._hash).val(),
                    Commands: commands,
                }, cb);
            } else {
                throw new Error('unexpected response code');
            }
        };

        self.sendCommandsEval({
            Hash: $(self._hash).val(),
            Commands: commands,
        }, cb);
    },

    sendCommandsEval: function (args, cb) {
        this.sendCommandsEvalFunc(JSON.stringify(args), function (_) { cb(JSON.parse(_)); });
    },

    sendCommandsEvalFunc: function (args, cb) {
        eval(this._push);
    },

};

_NXKit.Web.UI.View.registerClass('_NXKit.Web.UI.View', Sys.UI.Control);
