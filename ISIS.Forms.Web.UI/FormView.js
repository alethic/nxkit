Type.registerNamespace('ISIS.Forms.Web.UI');

ISIS.Forms.Web.UI.ValueChangedEventArgs = function (source, modelItemId, newValue, newValueHashCode)
{
    this._source = source;
    this._modelItemId = modelItemId;
    this._newValue = newValue;
    this._newValueHashCode = newValueHashCode;
};

ISIS.Forms.Web.UI.ValueChangedEventArgs.prototype =
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

ISIS.Forms.Web.UI.ValueChangedEventArgs.registerClass('ISIS.Forms.Web.UI.ValueChangedEventArgs', Sys.EventArgs);


ISIS.Forms.Web.UI.FormView = function (element)
{
    ISIS.Forms.Web.UI.FormView.initializeBase(this, [element]);
};

ISIS.Forms.Web.UI.FormView.prototype =
{

    initialize: function ()
    {
        ISIS.Forms.Web.UI.FormView.callBaseMethod(this, 'initialize');
    },

    dispose: function ()
    {
        ISIS.Forms.Web.UI.FormView.callBaseMethod(this, 'dispose');
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

ISIS.Forms.Web.UI.FormView.registerClass('ISIS.Forms.Web.UI.FormView', Sys.UI.Control);

if (typeof(Sys) !== 'undefined')
    Sys.Application.notifyScriptLoaded();
