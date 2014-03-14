Type.registerNamespace('NXKit.Web.UI');

NXKit.Web.UI.ValueChangedEventArgs = function (source, modelItemId, newValue, newValueHashCode)
{
    this._source = source;
    this._modelItemId = modelItemId;
    this._newValue = newValue;
    this._newValueHashCode = newValueHashCode;
};

NXKit.Web.UI.ValueChangedEventArgs.prototype =
{

    get_source: function ()
    {
        return this._source;
    },

    get_modelItemId: function ()
    {
        return this._modelItemId;
    },

    get_newValue: function () 
    {
        return this._newValue;
    },

    get_newValueHashCode: function () 
    {
        return this._newValueHashCode;
    }

};

NXKit.Web.UI.ValueChangedEventArgs.registerClass('NXKit.Web.UI.ValueChangedEventArgs', Sys.EventArgs);

NXKit.Web.UI.View = function (element)
{
    NXKit.Web.UI.View.initializeBase(this, [element]);
};

NXKit.Web.UI.View.prototype =
{

    initialize: function ()
    {
        NXKit.Web.UI.View.callBaseMethod(this, 'initialize');
    },

    dispose: function ()
    {
        NXKit.Web.UI.View.callBaseMethod(this, 'dispose');
    },

    add_valueChanged: function (handler)
    {
        this.get_events().addHandler('valueChanged', handler);
    },

    remove_valueChanged: function (handler)
    {
        this.get_events().removeHandler('valueChanged', handler);
    },

    raiseValueChanged: function (args)
    {
        var h = this.get_events().getHandler('valueChanged');
        if (h)
            h(this, args);
    },

};

NXKit.Web.UI.View.registerClass('NXKit.Web.UI.View', Sys.UI.Control);
Sys.Application.notifyScriptLoaded();
