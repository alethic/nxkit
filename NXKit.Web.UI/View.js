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

        Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(function (s, a) {
            self._pageLoadedHandler(s, a);
        });
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
                self._view.onPushRequest.add(function (data) {
                    self.onPushRequest(data);
                });
            }

            // update view with data
            self._view.data = $(self._data).val();
        }
    },

    onPushRequest: function (data) {
        var self = this;

        // generate event argument to pass to server
        var args = JSON.stringify({
            Action: 'Push',
            Save: $(self._save).val(),
            Args: {
                Data: data,
            },
        });

        // initiate server request
        var cb = function (args) {
            self._onPushRequestEnd(args);
        };

        eval(self._push);
    },

    _onPushRequestEnd: function (result) {
        var self = this;

        // result contains new save and data values
        var args = JSON.parse(result);
        $(self._save).val(args.Save);
        $(self._data).val(JSON.stringify(args.Data));

        self._view.data = $(self._data).val();
    },

    _pageLoadedHandler: function (sender, args) {
        console.debug('loaded');
    }

};

_NXKit.Web.UI.View.registerClass('_NXKit.Web.UI.View', Sys.UI.Control);
