Type.registerNamespace('NXKit.XForms.Web.UI');

NXKit.XForms.Web.UI.Select1MinimalControl = function (element)
{
    NXKit.XForms.Web.UI.Select1MinimalControl.initializeBase(this, [element]);

    this._formView = null;
    this._modelItemId = null;
    this._radComboBox = null;
};

NXKit.XForms.Web.UI.Select1MinimalControl.prototype =
{
    initialize: function ()
    {
        NXKit.XForms.Web.UI.Select1MinimalControl.callBaseMethod(this, 'initialize');
    },

    dispose: function ()
    {
        NXKit.XForms.Web.UI.Select1MinimalControl.callBaseMethod(this, 'dispose');
    },

    get_formView: function ()
    {
        return this._formView;
    },
    set_formView: function (value)
    {
        if (this._formView != value)
        {
            if (this._formView != undefined)
                this._formView.remove_valueChanged(Function.createDelegate(this, this._valueChangedHandler));

            this._formView = value;
            this._formView.add_valueChanged(Function.createDelegate(this, this._valueChangedHandler));
            this.raisePropertyChanged('formView');
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

    get_radComboBox: function()
    {
        return this._radComboBox;
    },

    set_radComboBox: function(value)
    {
        if (this._radComboBox != value)
        {
            if (this._radComboBox != null)
                this._radComboBox.remove_selectedIndexChanged(Function.createDelegate(this, this._onComboBoxSelectedIndexChangedHandler));

            this._radComboBox = value;
            this.raisePropertyChanged('radComboBox');

            this._radComboBox.add_selectedIndexChanged(Function.createDelegate(this, this._onComboBoxSelectedIndexChangedHandler));
        }
    },

    _valueChangedHandler: function (source, args)
    {
        // does this apply to us
        if (args.get_modelItemId() != this.get_modelItemId())
            return;

        // select item
        this.get_radComboBox().set_value(args.get_newValueHashCode());
    },

    _onComboBoxSelectedIndexChangedHandler: function (source, args)
    {
        var value = 0;
        var item = args.get_item();
        if (item != null)
            value = parseInt(item.get_value());

        this.get_formView().raiseValueChanged(new NXKit.Web.UI.ValueChangedEventArgs(self, this.get_modelItemId(), null, value));
    },
};

NXKit.XForms.Web.UI.Select1MinimalControl.registerClass('NXKit.XForms.Web.UI.Select1MinimalControl', Sys.UI.Control);

if (typeof(Sys) !== 'undefined')
    Sys.Application.notifyScriptLoaded();
