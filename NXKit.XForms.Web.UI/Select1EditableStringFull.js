Type.registerNamespace('NXKit.XForms.Web.UI');

NXKit.XForms.Web.UI.Select1EditableStringFull = function (element)
{
    NXKit.XForms.Web.UI.Select1EditableStringFull.initializeBase(this, [element]);

    this._view = null;
    this._modelItemId = null;
};

NXKit.XForms.Web.UI.Select1EditableStringFull.prototype =
{
    initialize: function ()
    {
        NXKit.XForms.Web.UI.Select1EditableStringFull.callBaseMethod(this, 'initialize');

        var self = this;

        View_jQuery(this.get_element()).find("input").each(function ()
        {
            $addHandler(this, "click", function()
            {
                self._onInputClickHandler(this);
            });
        });
    },

    dispose: function ()
    {
        NXKit.XForms.Web.UI.Select1EditableStringFull.callBaseMethod(this, 'dispose');
    },

    get_view: function ()
    {
        return this._view;
    },
    set_view: function (value)
    {
        if (this._view != value)
        {
            if (this._view != undefined)
                this._view.remove_valueChanged(Function.createDelegate(this, this._valueChangedHandler));

            this._view = value;
            this._view.add_valueChanged(Function.createDelegate(this, this._valueChangedHandler));
            this.raisePropertyChanged('view');
        }
    },

    get_modelItemId: function()
    {
        return this._modelItemId;
    },

    set_modelItemId: function(value)
    {
        if (this._modelItemId != value)
        {
            this._modelItemId = value;
            this.raisePropertyChanged('modelItemId');
        }
    },

    _valueChangedHandler: function (source, args)
    {
        // has the value of our node has changed
        if (args.get_modelItemId() != this.get_modelItemId())
            return;

        View_jQuery(this.get_element()).find("input").each(function ()
        {
            this.checked = parseInt(this.value) == args.get_newValueHashCode();
        });
    },

    _onInputClickHandler: function (input)
    {
        this.get_view().raiseValueChanged(new NXKit.Web.UI.ValueChangedEventArgs(self, this.get_modelItemId(), null, parseInt(input.value)));
    },
};

NXKit.XForms.Web.UI.Select1EditableStringFull.registerClass('NXKit.XForms.Web.UI.Select1EditableStringFull', Sys.UI.Control);

if (typeof(Sys) !== 'undefined')
    Sys.Application.notifyScriptLoaded();
