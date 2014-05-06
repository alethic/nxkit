Type.registerNamespace('_NXKit.Web.UI');

_NXKit.Web.UI.View = function (element, foo) {
    var self = this;
    _NXKit.Web.UI.View.initializeBase(self, [element]);

    self._view = null;
    self._data = null;
    self._save = null;
    self._body = null;
    self._push = null;
    self._messages = null;
    self._scripts = null;
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

    get_messages: function () {
        return JSON.stringify(this._messages);
    },

    set_messages: function (value) {
        this._messages = JSON.parse(value);
    },

    get_scripts: function () {
        return JSON.stringify(this._scripts);
    },

    set_scripts: function (value) {
        this._scripts = JSON.parse(value);
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

    logMessages: function (messages) {
        for (var i = 0; i < messages.length; i++) {
            var message = messages[i];
            if (message.Severity === 'Verbose' ||
                message.Severity === 'Information')
                console.debug(message.Text);
            if (message.Severity === 'Warning')
                console.warn(message.Text);
            if (message.Severity === 'Error')
                console.error(message.Text);
        }
    },

    runScripts: function (scripts) {
        for (var i = 0; i < scripts.length; i++) {
            var script = scripts[i];
            if (script != null)
                eval(script);
        }
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
            self._view.PushMessages(self._messages);

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

        // extract received messages
        var messages = args.Messages || [];
        self.logMessages(messages);

        // run received scripts
        var scripts = args.Scripts || [];
        self.runScripts(scripts);

        // update view
        self._view.Data = $(self._data).val();
        self._view.PushMessages(messages);

        wh();
    },

};

_NXKit.Web.UI.View.registerClass('_NXKit.Web.UI.View', Sys.UI.Control);
