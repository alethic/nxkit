Type.registerNamespace('_NXKit.Web.UI');

_NXKit.Web.UI.View = function (element) {
    _NXKit.Web.UI.View.initializeBase(this, [element]);

    this._object = new NXKit.Web.View(element);
};

_NXKit.Web.UI.View.prototype = {

    initialize: function () {
        _NXKit.Web.UI.View.callBaseMethod(this, 'initialize');
    },

    dispose: function () {
        _NXKit.Web.UI.View.callBaseMethod(this, 'dispose');
    },

    get_object: function () {
        return this._object;
    },

    get_model: function () {
        return this._object.model;
    },

    set_model: function (value) {
        this._object.model = value;
    },

};

_NXKit.Web.UI.View.registerClass('_NXKit.Web.UI.View', Sys.UI.Control);
