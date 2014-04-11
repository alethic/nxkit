Type.registerNamespace('_NXKit.Web.UI');

_NXKit.Web.UI.View = function (element, foo) {
    var self = this;
    _NXKit.Web.UI.View.initializeBase(self, [element]);

    self._view = null;
    self._data = null;
    self._save = null;
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
                self._view = new NXKit.Web.View(self._body);
                self._view.CallbackRequest.add(function (data, wh) {
                    self.onCallbackRequest(data, wh);
                });
            }

            // update view with data
            self._view.Data = $(self._data).val();

            // when the form is submitted, ensure the data field is updated
            $(self.get_element()).parents('form').submit(function (event) {
                $(self._data).val(JSON.stringify(self._view.Data));
            });
        }
    },

    onBeginRequest: function () {

    },

    onCallbackRequest: function (data, wh) {
        var self = this;

        // generate event argument to pass to server
        var args = JSON.stringify({
            Action: data.Action,
            Save: $(self._save).val(),
            Args: data.Args,
        });

        // initiate server request
        var cb = function (args) {
            self.onCallbackRequestEnd(args, wh);
        };

        eval(self._push);
    },

    onCallbackRequestEnd: function (result, wh) {
        var self = this;

        // result contains new save and data values
        var args = JSON.parse(result);
        $(self._save).val(args.Save);
        $(self._data).val(JSON.stringify(args.Data));

        self._view.Data = $(self._data).val();

        wh();
    },

};

_NXKit.Web.UI.View.registerClass('_NXKit.Web.UI.View', Sys.UI.Control);
